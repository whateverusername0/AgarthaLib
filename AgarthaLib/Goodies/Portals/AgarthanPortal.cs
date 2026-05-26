using AgarthaLib.Attributes;
using AgarthaLib.Collision;
using AgarthaLib.EventSystem.StaticDispatchers;
using AgarthaLib.Extensions;
using AgarthaLib.Goodies.Pause;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace AgarthaLib.Goodies.Portals
{
    public class AgarthanPortal : PausedBehavior
    {
        private Camera _mainCamera => Camera.main;

        [Header("Properties")]
        public AgarthanPortal LinkedPortal;
        public Transform NormalVisible;
        public Transform NormalInvisible;
        [EditorReadOnly] public List<AgarthanPortal> VisiblePortals = new();

        public Vector3 InwardsForward => NormalVisible.forward;
        public Vector3 OutwardsForward => NormalInvisible.forward;

        [Header("Collision")]
        public bool CanPassThrough = true;
        public float VelocityOverride = 1.5f;
        public List<Type> PassthroughTypes = new()
        {
            typeof(Rigidbody),
            typeof(Rigidbody2D),
            typeof(CharacterController)
        };
        [SerializeField, EditorReadOnly] private List<Transform> _collidingObjects = new();
        [SerializeField, EditorReadOnly] private List<Transform> _objectRemovalQueue = new();

        [Header("Rendering")]
        public bool RenderingEnabled = true;
        public int Depth = 2;
        public Color DepthColorTint = Color.black;
        [ValidateNull(traverse: true)] public Camera Camera;
        public Renderer MeshRenderer;
        [SerializeField, EditorReadOnly] private RenderTexture _renderTexture;
        [SerializeField, EditorReadOnly] private Material _material;

        [Header("Debug")]
        [NonSerialized] public Vector4 VectorPlane;
        [SerializeField, EditorReadOnly] private bool _shouldRender = false;

        public bool IsBeingRendered => MeshRenderer.isVisible
                && MeshRenderer.IsVisibleFrom(_mainCamera);

        public void EnableRendering(bool enable) => RenderingEnabled = enable;

        protected override void Start()
        {
            base.Start();

            ResolveDependencies();

            SubscribeGlobalEvent<ResolutionChangedEvent>(OnResolutionChanged);

            SubscribeEvent<CollisionEnterEvent>(OnCollisionEnterEvent);
            SubscribeEvent<CollisionExitEvent>(OnCollisionExitEvent);
        }

        private void OnResolutionChanged(ref ResolutionChangedEvent args)
        {
            // change rt's resolution
            ResolveDependencies(forced: true);
        }

        private void OnCollisionEnterEvent(GameObject invoker, ref CollisionEnterEvent args)
        {
            var other = args.Other.transform.root;
            var compRegistry = other.GetComponents<Component>();
            // check if it has any valid components for it to pass through.
            if (compRegistry.Where(q => PassthroughTypes.Any(w => q.GetType() == w)).Count() == 0)
                return;

            // entering the portal from behind - don't teleport, let the entity pass
            if (GetDotProduct(other.position) < 0f)
                return;

            if (!_collidingObjects.Contains(other))
                _collidingObjects.Add(other);
        }

        private void OnCollisionExitEvent(GameObject invoker, ref CollisionExitEvent args)
        {
            var other = args.Other.transform.root;
            if (_collidingObjects.Contains(other))
                _collidingObjects.Remove(other);
        }

        protected override void UnpausedLateUpdate()
        {
            _shouldRender = RenderingEnabled && IsBeingRendered;
            if (!_shouldRender)
                return;

            var mc = _mainCamera.transform;
            RecursiveRender(mc.position, mc.rotation, Camera, 0, Depth);
        }

        protected override void LateFixedUpdate()
        {
            base.LateFixedUpdate();

            if (LinkedPortal == null || !CanPassThrough)
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
                if (GetDotProduct(item.position) > 0)
                    continue;

                var newPos = TransformPosition(item.position);
                var newRot = TransformRotation(item.rotation);
                item.SetPositionAndRotation(newPos, newRot);

                RaiseEvent<PortalTeleportedEvent>(item.gameObject, new(this, LinkedPortal, newPos, newRot));

                _objectRemovalQueue.Add(item);
            }

            foreach (var item in _objectRemovalQueue)
                _collidingObjects.Remove(item);
        }

        private void OnDestroy()
        {
            if (_renderTexture != null)
            {
                _renderTexture.Release();
                Destroy(_renderTexture);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(NormalVisible.position, NormalVisible.position + NormalVisible.forward);
            Gizmos.DrawLine(NormalVisible.position, NormalVisible.position + NormalVisible.up);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(NormalInvisible.position, NormalInvisible.position + NormalInvisible.forward);
            Gizmos.DrawLine(NormalInvisible.position, NormalInvisible.position + NormalInvisible.up);

            Gizmos.color = Color.yellow;
            if (LinkedPortal != null && LinkedPortal != this)
                Gizmos.DrawLine(transform.position, LinkedPortal.transform.position);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            if (VisiblePortals.Count == 0)
                return;

            foreach (var visible in VisiblePortals.Where(q => q != null))
                Gizmos.DrawLine(transform.position, visible.transform.position);
        }

        [ContextMenu("Resolve visible portals")]
        public void ResolveVisiblePortals()
        {
            Predicate<Collider> overlapPred =
                (q) => q.transform == transform || !q.isTrigger || !q.HasComponent<AgarthanPortal>();

            if (!PortalAwarePhysics.OverlapHemisphereUnoccluded(transform.position, NormalVisible.forward, 50f, out var hits, overlapPred))
                return;

            var valid = hits.Where(q => q.HasComponent<AgarthanPortal>()).ToList();
            VisiblePortals = valid.ConvertAll(q => q.GetComponent<AgarthanPortal>());
            // just in case.
            // EDIT: the case is real.
            VisiblePortals.Remove(this);
        }

        #region Logic

        private void ResolveDependencies(bool forced = false)
        {
            if (_renderTexture == null || forced)
            {
                if (_renderTexture != null)
                {
                    _renderTexture.Release();
                    _renderTexture.DiscardContents();
                }

                _renderTexture = new(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
                _renderTexture.name = this.name;
                _renderTexture.Create();
            }

            Camera.targetTexture = _renderTexture;
            Camera.farClipPlane = _mainCamera.farClipPlane;

            _material = MeshRenderer.material;
            _material.mainTexture = _renderTexture;

            var plane = new Plane(NormalVisible.forward, transform.position);
            VectorPlane = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);
        }

        public void RecursiveRender(Vector3 pos, Quaternion rot, Camera cam, int depth, int maxDepth)
        {
            if (LinkedPortal == null)
                return;

            var virtualPosition = TransformPosition(pos);
            var virtualRotation = TransformRotation(rot);

            if (LinkedPortal.VisiblePortals.Count <= 0)
            {
                RenderPortalCamera(cam, virtualPosition, virtualRotation);
                _material.mainTexture = _renderTexture;
                return;
            }

            if (depth >= maxDepth)
                return;

            foreach (var visiblePortal in LinkedPortal.VisiblePortals)
            {
                if (visiblePortal == null
                || !visiblePortal.MeshRenderer.IsVisibleFrom(cam))
                    continue;

                visiblePortal.RecursiveRender(virtualPosition, virtualRotation, cam, depth + 1, maxDepth);
            }

            // consider it the shallowest and render
            RenderPortalCamera(cam, virtualPosition, virtualRotation);

            _material.mainTexture = _renderTexture;
            _material.color = DepthColorTint;

            var d = (float)depth / (float)maxDepth;
            var blend = _material.GetFloat("_Blend");
            _material.SetFloat("_Blend", Mathf.Lerp(blend, d, Time.deltaTime * 3.5f));
        }

        public void RenderPortalCamera(Camera cam, Vector3 position, Quaternion rotation)
        {
            cam.transform.SetPositionAndRotation(position, rotation);

            var clip = Matrix4x4.Transpose(Matrix4x4.Inverse(cam.worldToCameraMatrix)) * LinkedPortal.VectorPlane;
            var opm = cam.CalculateObliqueMatrix(clip);
            cam.projectionMatrix = opm;
            cam.targetTexture = _renderTexture;

            cam.Render();
        }

        #endregion

        #region API

        public static Vector3 TransformPosition(AgarthanPortal a, AgarthanPortal b, Vector3 position)
            => b.NormalInvisible.TransformPoint(a.NormalVisible.InverseTransformPoint(position));

        public Vector3 TransformPosition(Vector3 position)
            => TransformPosition(this, LinkedPortal, position);

        public static Vector3 TransformDirection(AgarthanPortal a, AgarthanPortal b, Vector3 direction)
            => b.NormalInvisible.TransformDirection(a.NormalVisible.InverseTransformDirection(direction));

        public Vector3 TransformDirection(Vector3 direction)
            => TransformDirection(this, LinkedPortal, direction);

        public Quaternion GetRotationDelta(AgarthanPortal a, AgarthanPortal b)
            => b.NormalInvisible.rotation * Quaternion.Inverse(a.NormalVisible.rotation);

        public Quaternion GetRotationDelta()
            => GetRotationDelta(this, LinkedPortal);

        public static Quaternion TransformRotation(AgarthanPortal a, AgarthanPortal b, Quaternion rotation)
            => b.NormalInvisible.rotation * Quaternion.Inverse(a.NormalVisible.rotation) * rotation;

        public Quaternion TransformRotation(Quaternion rotation)
            => TransformRotation(this, LinkedPortal, rotation);

        public float GetDotProduct(Vector3 position)
        {
            var direction = (position - transform.position).normalized;
            var dot = Vector3.Dot(direction, NormalVisible.forward);
            return dot;
        }

        #endregion
    }
}
