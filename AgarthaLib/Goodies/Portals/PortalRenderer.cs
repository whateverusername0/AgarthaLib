using AgarthaLib.Attributes;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Goodies.Portals
{
    public class PortalRenderer : AgarthanBehaviour
    {
        private Camera _mainCamera => Camera.main;
        public Camera Camera;

        [EditorReadOnly] public List<PortalOcclusionVolume> OcclusionVolumes = new();
        public PortalOcclusionVolume CurrentVolume;

        public int MaxDepth = 2;

        protected override void Start()
        {
            base.Start();

            ResolveVolumes();
        }

        [ContextMenu("Resolve volumes")] public void ResolveVolumes()
        {
            var volumesPool = FindObjectsByType<PortalOcclusionVolume>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID).ToList();
            OcclusionVolumes = OcclusionVolumes.Count == 0 ? volumesPool : OcclusionVolumes;
        }

        private void LateUpdate()
        {
            var currentVolume = OcclusionVolumes.Where(q => q.Collider.bounds.Contains(Camera.transform.position)).FirstOrDefault();
            var volume = currentVolume == null ? PortalOcclusionVolume.GetCurrentVolume(_mainCamera) : currentVolume;

            // don't bother
            if (volume == null) return;

            foreach (var portal in volume.OccludedPortals)
            {
                if (!portal.MeshRenderer.isVisible
                || !portal.MeshRenderer.IsVisibleFrom(_mainCamera)
                || !PortalOcclusionVolume.IsInSameVolume(_mainCamera, portal))
                    continue;

                portal.RecursiveRender(_mainCamera.transform.position, _mainCamera.transform.rotation,
                    out _, out _, Camera, 0, MaxDepth);
            }
        }
    }
}
