using AgarthaLib.Attributes;
using AgarthaLib.Collision;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace AgarthaLib.Goodies.Portals
{
    public class AgarthanPortal : AgarthanBehaviour
    {
        private struct VisiblePortalResource
        {
            public AgarthanPortal VisiblePortal;
            public Texture OriginalTexture;

            public VisiblePortalResource(AgarthanPortal visiblePortal, Texture originalTexture)
            {
                VisiblePortal = visiblePortal;
                OriginalTexture = originalTexture;
            }
        }

        private Camera _mainCamera => Camera.main;

        public AgarthanPortal LinkedPortal;
        public Transform NormalVisible;
        public Transform NormalInvisible;
        [EditorReadOnly] public List<AgarthanPortal> VisiblePortals = new();

        [Header("Collision")]
        public bool CanPassThrough = true;
        public float VelocityThreshold = 1f;
        public float VelocityModifier = 1.5f;
        public List<Type> PassthroughTypes = new()
        {
            typeof(Rigidbody),
            typeof(Rigidbody2D),
            typeof(CharacterController)
        };
        [SerializeField, EditorReadOnly] private List<Transform> _collidingObjects = new();
        [SerializeField, EditorReadOnly] private List<Transform> _objectRemovalQueue = new();

        [Header("Rendering")]
        [ValidateNull(traverse: true)] public Camera Camera;
        [SerializeField, EditorReadOnly] private RenderTexture _renderTexture;
        public Renderer MeshRenderer;
        public Texture FallbackDepthTexture;
        public bool InfiniteDepth = true;
        public int MaxDepth = 2;
        public int Downscale = 2;

        [Header("Debug")]
        [NonSerialized] public Vector4 VectorPlane;
        [SerializeField, EditorReadOnly] private bool _isBeingRendered = false;
        public bool IsBeingRendered => MeshRenderer.isVisible
                && MeshRenderer.IsVisibleFrom(_mainCamera)
                && PortalOcclusionVolume.IsInSameVolume(_mainCamera, this);

        private readonly UniversalRenderPipeline.SingleCameraRequest _request = new();

        protected override void Start()
        {
            base.Start();

            ResolveDependencies();

            SubscribeEvent<CollisionEnterEvent>(OnCollisionEnterEvent);
            SubscribeEvent<CollisionExitEvent>(OnCollisionExitEvent);
        }

        private void ResolveDependencies()
        {
            if (_renderTexture == null)
            {
                _renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
                _renderTexture.name = this.name;
                _renderTexture.Create();
            }
            Camera.targetTexture = _renderTexture;
            MeshRenderer.sharedMaterial.mainTexture = _renderTexture;

            var plane = new Plane(NormalVisible.forward, transform.position);
            VectorPlane = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);
        }

        private void OnDestroy()
        {
            if (_renderTexture != null)
            {
                _renderTexture.Release();
                Destroy(_renderTexture);
            }
        }

        private void LateUpdate()
        {
            _isBeingRendered = IsBeingRendered;
            if (!_isBeingRendered)
                return;

            var mc = _mainCamera.transform;
            RecursiveRender(mc.position, mc.rotation, out _, Camera, 0);
        }

        public void RecursiveRender(Vector3 refPos, Quaternion refRot,
            out Texture originalTexture,
            Camera cam, int depth)
        {
            var virtualPosition = TransformPosition(this, LinkedPortal, refPos);
            var virtualRotation = TransformRotation(this, LinkedPortal, refRot);
            cam.transform.SetPositionAndRotation(virtualPosition, virtualRotation);

            var clip = Matrix4x4.Transpose(Matrix4x4.Inverse(cam.worldToCameraMatrix)) * LinkedPortal.VectorPlane;
            var obliqueProjectionMatrix = cam.CalculateObliqueMatrix(clip);
            cam.projectionMatrix = obliqueProjectionMatrix;

            var vprList = new List<VisiblePortalResource>();
            if (depth < MaxDepth && LinkedPortal.VisiblePortals.Count > 0)
            {
                foreach (var visiblePortal in LinkedPortal.VisiblePortals)
                {
                    if (!visiblePortal.MeshRenderer.IsVisibleFrom(cam))
                        continue;

                    visiblePortal.RecursiveRender(
                        virtualPosition, virtualRotation,
                        out var visibleTexture,
                        cam, depth + 1);

                    vprList.Add(new(visiblePortal, visibleTexture));
                }
            }
            else
            {
                foreach (var visiblePortal in LinkedPortal.VisiblePortals)
                {
                    var visibleTexture = MeshRenderer.material.mainTexture;
                    MeshRenderer.material.mainTexture = FallbackDepthTexture;
                    vprList.Add(new(visiblePortal, visibleTexture));
                }
            }

            cam.targetTexture = _renderTexture;
            cam.transform.SetPositionAndRotation(virtualPosition, virtualRotation);
            cam.projectionMatrix = obliqueProjectionMatrix;

            // Camera.Render().
            if (UnityEngineExtensions.TryGetUsedPipeline(out _))
                RenderPipeline.SubmitRenderRequest(cam, _request);
            else cam.Render();

            foreach (var resource in vprList)
            {
                // Reset to original texture
                // So that it will remain correct if the visible portal is still expecting to be rendered
                // on another camera but has already rendered its texture.
                // Originally the texture may be overriden by other renders.
                resource.VisiblePortal.MeshRenderer.material.mainTexture = resource.OriginalTexture;
            }

            // Must be after camera render, in case it renders itself
            // (in which the texture must not be replaced before rendering itself)
            // Must be after restore, in case it restores its own old texture
            // (in which the new texture must take precedence)
            originalTexture = MeshRenderer.material.mainTexture;
            MeshRenderer.material.mainTexture = _renderTexture;
        }

        protected override void LateFixedUpdate()
        {
            base.LateFixedUpdate();

            if (!CanPassThrough)
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

                if (item.HasComponent<CharacterController>())
                    UnityEngine.Physics.SyncTransforms();
                // CharacterController controls velocity in their own logic.

                if (item.TryGetComponent<Rigidbody>(out var rb))
                {
                    item.rotation = Quaternion.LookRotation(item.forward, Vector3.up);
                    rb.linearVelocity = TransformDirection(this, LinkedPortal, rb.linearVelocity);
                    if (rb.linearVelocity.magnitude <= VelocityThreshold)
                        rb.linearVelocity *= VelocityModifier;
                }

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

            foreach (var visible in VisiblePortals)
                Gizmos.DrawLine(transform.position, visible.transform.position);
        }

        private void OnCollisionEnterEvent(GameObject invoker, ref CollisionEnterEvent args)
        {
            var other = args.Other.transform.root;
            var compRegistry = other.GetComponents<Component>();
            // check if it has any valid components for it to pass through.
            if (compRegistry.Where(q => PassthroughTypes.Any(w => q.GetType() == w)).Count() == 0)
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

        [ContextMenu("Resolve visible portals")] public void ResolveVisiblePortals()
        {
            Predicate<Collider> overlapPred =
                (q) => q.transform == transform || !q.isTrigger || !q.HasComponent<AgarthanPortal>();

            if (!Extensions.Physics.OverlapHalfSphereUnoccluded(transform.position, NormalVisible.forward, 50f, out var hits, overlapPred))
                return;

            var valid = hits.Where(q => q.HasComponent<AgarthanPortal>()).ToList();
            VisiblePortals = valid.ConvertAll(q => q.GetComponent<AgarthanPortal>());
            // just in case.
            // EDIT: the case is real.
            VisiblePortals.Remove(this);
        }

        public static Vector3 TransformPosition(AgarthanPortal a, AgarthanPortal b, Vector3 position)
            => b.NormalInvisible.TransformPoint(a.NormalVisible.InverseTransformPoint(position));

        public static Vector3 TransformDirection(AgarthanPortal a, AgarthanPortal b, Vector3 direction)
            => b.NormalInvisible.TransformDirection(a.NormalVisible.InverseTransformDirection(direction));

        public Quaternion GetRotationDelta()
            => LinkedPortal.NormalInvisible.rotation * Quaternion.Inverse(NormalVisible.rotation);

        public static Quaternion TransformRotation(AgarthanPortal a, AgarthanPortal b, Quaternion rotation)
            => b.NormalInvisible.rotation * Quaternion.Inverse(a.NormalVisible.rotation) * rotation;
    }
}
