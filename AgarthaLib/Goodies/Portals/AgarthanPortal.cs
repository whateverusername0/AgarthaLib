using AgarthaLib.Attributes;
using AgarthaLib.ECS.Systems;
using AgarthaLib.Extensions;
using AgarthaLib.Goodies.Pause;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Goodies.Portals
{
    [Tooltip("A basic implementation of a PortalBase with dynamic rendering.")]
    public class AgarthanPortal : PortalBase
    {
        protected PauseManager _pause => PauseManager.Instance;
        protected Camera _mainCamera => Camera.main;

        [ValidateNull(traverse: true)] public Camera Camera;
        public Renderer MeshRenderer;
        [EditorReadOnly] public List<AgarthanPortal> VisiblePortals = new();

        [Header("Rendering")]
        public bool RenderingEnabled = true;
        public bool RenderWhilePaused = true;
        public int Depth = 2;
        public Color DepthColorTint = Color.black;
        
        [Header("Debug")]
        [NonSerialized] public Vector4 VectorPlane;
        [SerializeField, EditorReadOnly] protected bool _shouldRender = false;
        [SerializeField, EditorReadOnly] protected Material _material;

        [SerializeField, EditorReadOnly] protected RenderTexture _renderTexture;
        public RenderTexture RenderTexture => _renderTexture;

        public bool IsBeingRendered => MeshRenderer.isVisible
                && MeshRenderer.IsVisibleFrom(_mainCamera);

        public void EnableRendering(bool enable) => RenderingEnabled = enable;

        protected override void Start()
        {
            base.Start();

            ResolveDependencies();

            SubscribeGlobalEvent<ResolutionChangedEvent>(OnResolutionChanged);
        }

        protected virtual void OnResolutionChanged(ref ResolutionChangedEvent args)
        {
            // changes rt's resolution
            ResolveDependencies();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (!RenderWhilePaused && _pause.Paused)
                return;

            _shouldRender = RenderingEnabled && IsBeingRendered;
            if (!_shouldRender)
                return;

            var mc = _mainCamera.transform;
            RecursiveRender(mc.position, mc.rotation, Camera, 0, Depth);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_renderTexture != null)
            {
                _renderTexture.Release();
                Destroy(_renderTexture);
            }
        }

        protected virtual void OnDrawGizmosSelected()
        {
            // draws visible portals
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

        protected virtual void ResolveDependencies()
        {
            if (_renderTexture != null)
            {
                _renderTexture.Release();
                _renderTexture.DiscardContents();
            }

            _renderTexture = new(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            _renderTexture.name = this.name;
            _renderTexture.Create();

            Camera.targetTexture = _renderTexture;
            Camera.farClipPlane = _mainCamera.farClipPlane;

            _material = MeshRenderer.material;
            _material.mainTexture = _renderTexture;

            var plane = new Plane(NormalVisible.forward, transform.position);
            VectorPlane = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);
        }

        public virtual void RecursiveRender(Vector3 pos, Quaternion rot, Camera cam, int depth, int maxDepth)
        {
            if (LinkedPortal == null)
                return;

            var virtualPosition = TransformPosition(pos);
            var virtualRotation = TransformRotation(rot);

            if (LinkedPortal is not AgarthanPortal { } linked
            || linked.VisiblePortals.Count <= 0)
            {
                RenderPortalCamera(cam, virtualPosition, virtualRotation);
                return;
            }

            if (depth >= maxDepth)
                return;

            foreach (var visiblePortal in linked.VisiblePortals)
            {
                if (visiblePortal == null
                || !visiblePortal.MeshRenderer.IsVisibleFrom(cam))
                    continue;

                visiblePortal.RecursiveRender(virtualPosition, virtualRotation, cam, depth + 1, maxDepth);
            }

            RenderPortalCamera(cam, virtualPosition, virtualRotation);

            _material.color = DepthColorTint;

            var d = (float)depth / (float)maxDepth;
            var blend = _material.GetFloat("_Blend");
            _material.SetFloat("_Blend", d);
        }

        public virtual void RenderPortalCamera(Camera cam, Vector3 position, Quaternion rotation)
        {
            cam.transform.SetPositionAndRotation(position, rotation);

            if (LinkedPortal is AgarthanPortal { } linked)
            {
                var clip = Matrix4x4.Transpose(Matrix4x4.Inverse(cam.worldToCameraMatrix)) * linked.VectorPlane;
                var opm = cam.CalculateObliqueMatrix(clip);
                cam.projectionMatrix = opm;
            }
            cam.targetTexture = _renderTexture;

            cam.Render();
        }
    }
}
