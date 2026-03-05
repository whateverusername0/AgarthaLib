using AgarthaLib.Attributes;
using AgarthaLib.Collision;
using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Goodies.Portals
{
    public class PortalRenderer : AgarthanBehaviour
    {
        private Camera _mainCamera => Camera.main;

        public PortalRenderer LinkedPortal;
        [Header("Collision")]
        public bool PortalFunctionality = true;
        [SerializeField, EditorReadOnly] private List<Transform> _collidingObjects = new();
        [SerializeField, EditorReadOnly] private List<Transform> _objectRemovalQueue = new();

        [Header("Rendering")]
        public Transform NormalVisible;
        public Transform NormalInvisible;
        [ValidateNull(traverse: true)] public Camera Camera;
        public Renderer ViewthroughRenderer;
        public int RenderDepth = 8;

        private RenderTexture _rt;
        [EditorReadOnly] public Vector4 VectorPlane;

        protected override void Start()
        {
            base.Start();

            _rt = _rt == null ? new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default) : _rt;
            Camera.targetTexture = _rt;
            ViewthroughRenderer.material.mainTexture = _rt;

            var plane = new Plane(NormalVisible.forward, transform.position);
            VectorPlane = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);

            SubscribeEvent<CollisionEnterEvent>(OnCollisionEnterEvent);
            SubscribeEvent<CollisionExitEvent>(OnCollisionExitEvent);
        }

        private void OnDestroy()
        {
            _rt.Release();
            Destroy(_rt);
        }

        private void LateUpdate()
        {
            // Not visible? Fuck off.
            if (!ViewthroughRenderer.isVisible)
                return;

            // No link? Turn into a mirror I guess.
            if (LinkedPortal == null)
                LinkedPortal = this;

            var mct = _mainCamera.transform;
            var virtualPosition = TransformPosition(this, LinkedPortal, mct.position);
            var virtualRotation = TransformRotation(this, LinkedPortal, mct.rotation);

            Camera.transform.SetPositionAndRotation(virtualPosition, virtualRotation);

            // Calculate projection matrix
            // Set portal camera projection matrix to clip walls between target portal and portal camera
            // Inherits main camera near/far clip plane and FOV settings
            var clip = Matrix4x4.Transpose(Matrix4x4.Inverse(Camera.worldToCameraMatrix)) * LinkedPortal.VectorPlane;
            Camera.projectionMatrix = _mainCamera.CalculateObliqueMatrix(clip);

            Camera.Render();
        }

        protected override void LateFixedUpdate()
        {
            base.LateFixedUpdate();

            if (!PortalFunctionality)
                return;

            _objectRemovalQueue.Clear();
            foreach (var item in _collidingObjects)
            {
                if (item == null)
                {
                    _objectRemovalQueue.Add(item);
                    continue;
                }

                // If the dot product is more than 0
                // that means that the portal-to-object direction
                // and the visible normal direction is on the same side
                var direction = (item.position - transform.position).normalized;
                var dot = Vector3.Dot(direction, NormalVisible.forward);
                if (dot > 0) continue;

                var newPos = TransformPosition(this, LinkedPortal, item.position);
                var newRot = TransformRotation(this, LinkedPortal, item.rotation);
                item.SetPositionAndRotation(newPos, newRot);

                RaiseEvent<PortalTeleportedEvent>(item.gameObject, new(this, LinkedPortal, newPos, newRot));

                _objectRemovalQueue.Add(item);
            }

            foreach (var item in _objectRemovalQueue)
                _collidingObjects.Remove(item);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(NormalVisible.position, NormalVisible.position + NormalVisible.forward);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(NormalInvisible.position, NormalInvisible.position + NormalInvisible.forward);
        }

        private void OnCollisionEnterEvent(GameObject invoker, ref CollisionEnterEvent args)
        {
            var other = args.Other.transform.root;
            if (!_collidingObjects.Contains(other))
                _collidingObjects.Add(other);
        }

        private void OnCollisionExitEvent(GameObject invoker, ref CollisionExitEvent args)
        {
            var other = args.Other.transform.root;
            if (_collidingObjects.Contains(other))
                _collidingObjects.Remove(other);
        }

        public static Vector3 TransformPosition(PortalRenderer a, PortalRenderer b, Vector3 mainPos)
            => b.NormalInvisible.TransformPoint(a.NormalVisible.InverseTransformPoint(mainPos));

        public static Quaternion TransformRotation(PortalRenderer a, PortalRenderer b, Quaternion mainRot)
            => b.NormalInvisible.rotation * Quaternion.Inverse(a.NormalVisible.rotation) * mainRot;
    }
}
