using AgarthaLib.Attributes;
using AgarthaLib.Collision;
using AgarthaLib.Extensions;
using AgarthaLib.Goodies.Rendering;
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
            public RenderTexturePoolItem PoolItem;
            public Texture OriginalTexture;

            public VisiblePortalResource(AgarthanPortal visiblePortal, RenderTexturePoolItem poolItem, Texture originalTexture)
            {
                VisiblePortal = visiblePortal;
                PoolItem = poolItem;
                OriginalTexture = originalTexture;
            }
        }

        private Camera _mainCamera => Camera.main;

        public AgarthanPortal LinkedPortal;
        public Transform NormalVisible;
        public Transform NormalInvisible;
        [EditorReadOnly] public List<AgarthanPortal> VisiblePortals = new();

        [Header("Collision")]
        public bool PortalFunctionality = true;
        [SerializeField, EditorReadOnly] private List<Transform> _collidingObjects = new();
        [SerializeField, EditorReadOnly] private List<Transform> _objectRemovalQueue = new();

        [Header("Rendering")]
        public Renderer MeshRenderer;
        public Texture FallbackDepthTexture;
        public int MaxDepthOverride = 2;
        [EditorReadOnly] public Vector4 VectorPlane;

        [Header("Legacy Rendering")]
        public bool UseLegacyRendering = false;
        [ValidateNull(traverse: true)] public Camera Camera;
        [SerializeField, EditorReadOnly] private RenderTexture _rt;

        private UniversalRenderPipeline.SingleCameraRequest _request = new();

        protected override void Start()
        {
            base.Start();

            if (UseLegacyRendering)
            {
                if (_rt == null)
                {
                    _rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default);
                    _rt.Create();
                }
                Camera.targetTexture = _rt;
                MeshRenderer.material.mainTexture = _rt;
            }

            var plane = new Plane(NormalVisible.forward, transform.position);
            VectorPlane = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);

            SubscribeEvent<CollisionEnterEvent>(OnCollisionEnterEvent);
            SubscribeEvent<CollisionExitEvent>(OnCollisionExitEvent);
        }

        private void OnDestroy()
        {
            if (UseLegacyRendering && _rt != null)
            {
                _rt.Release();
                Destroy(_rt);
            }
        }

        private void LateUpdate()
        {
            if (!UseLegacyRendering) return;

            // Not visible? Fuck off.
            if (!MeshRenderer.isVisible
            || !MeshRenderer.IsVisibleFrom(_mainCamera)
            || !PortalOcclusionVolume.IsInSameVolume(_mainCamera, this))
                return;

            Render(Camera, LinkedPortal);
        }

        public void Render(Camera cam, AgarthanPortal target, int depth = 0, AgarthanPortal caller = null)
        {
            // No link? Turn into a mirror I guess.
            if (target == null) target = this;

            var virtualPosition = _mainCamera.transform.position;
            var virtualRotation = _mainCamera.transform.rotation;
            cam.transform.SetPositionAndRotation(virtualPosition, virtualRotation);

            // Calculate projection matrix
            // Set portal camera projection matrix to clip walls between target portal and portal camera
            // Inherits main camera near/far clip plane and FOV settings
            var clip = Matrix4x4.Transpose(Matrix4x4.Inverse(cam.worldToCameraMatrix)) * target.VectorPlane;
            cam.projectionMatrix = _mainCamera.CalculateObliqueMatrix(clip);

            //cam.Render();
            RenderPipeline.SubmitRenderRequest(cam, _request);
        }

        public void RecursiveRender(Vector3 refPos, Quaternion refRot,
            out RenderTexturePoolItem tempItem, out Texture originalTexture,
            Camera cam, int depth, int maxDepth)
        {
            var virtualPosition = TransformPosition(this, LinkedPortal, refPos);
            var virtualRotation = TransformRotation(this, LinkedPortal, refRot);
            cam.transform.SetPositionAndRotation(virtualPosition, virtualRotation);

            var clip = Matrix4x4.Transpose(Matrix4x4.Inverse(cam.worldToCameraMatrix)) * LinkedPortal.VectorPlane;
            var obliqueProjectionMatrix = _mainCamera.CalculateObliqueMatrix(clip);
            cam.projectionMatrix = obliqueProjectionMatrix;

            var vprList = new List<VisiblePortalResource>();
            var actualMaxDepth = LinkedPortal.MaxDepthOverride > 0 ? LinkedPortal.MaxDepthOverride : maxDepth;
            if (depth < actualMaxDepth && LinkedPortal.VisiblePortals.Count > 0)
            {
                foreach (var visiblePortal in LinkedPortal.VisiblePortals)
                {
                    if (!visiblePortal.MeshRenderer.isVisible || !visiblePortal.MeshRenderer.IsVisibleFrom(_mainCamera))
                        continue;

                    visiblePortal.RecursiveRender(
                        virtualPosition, virtualRotation,
                        out var visibleTempItem,
                        out var visibleTexture,
                        cam, depth + 1, maxDepth);

                    vprList.Add(new(visiblePortal, visibleTempItem, visibleTexture));
                }
            }
            else
            {
                foreach (var visiblePortal in LinkedPortal.VisiblePortals)
                {
                    var visibleTexture = MeshRenderer.material.mainTexture;
                    MeshRenderer.material.mainTexture = FallbackDepthTexture;
                    vprList.Add(new(visiblePortal, null, visibleTexture));
                }
            }

            tempItem = RenderTexturePool.Instance.GetTexture();

            cam.targetTexture = tempItem.RenderTexture;
            cam.transform.SetPositionAndRotation(virtualPosition, virtualRotation);
            cam.projectionMatrix = obliqueProjectionMatrix;

            //cam.Render();
            RenderPipeline.SubmitRenderRequest(cam, _request);

            foreach (var resource in vprList)
            {
                // Reset to original texture
                // So that it will remain correct if the visible portal is still expecting to be rendered
                // on another camera but has already rendered its texture. Originally the texture may be overriden by other renders.
                resource.VisiblePortal.MeshRenderer.material.mainTexture = resource.OriginalTexture;

                // Release temp render texture
                if (resource.PoolItem != null)
                    RenderTexturePool.Instance.ReleaseTexture(resource.PoolItem);
            }

            // Must be after camera render, in case it renders itself (in which the texture must not be replaced before rendering itself)
            // Must be after restore, in case it restores its own old texture (in which the new texture must take precedence)
            originalTexture = MeshRenderer.material.mainTexture;
            MeshRenderer.material.mainTexture = tempItem.RenderTexture;
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

                if (item.HasComponent<CharacterController>())
                    UnityEngine.Physics.SyncTransforms();

                RaiseEvent<PortalTeleportedEvent>(item.gameObject, new(this, LinkedPortal, newPos, newRot));

                _objectRemovalQueue.Add(item);
            }

            foreach (var item in _objectRemovalQueue)
                _collidingObjects.Remove(item);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

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

            if (!Extensions.Physics.OverlapSphereUnoccluded(transform.position, 10f, out var hits, overlapPred))
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
