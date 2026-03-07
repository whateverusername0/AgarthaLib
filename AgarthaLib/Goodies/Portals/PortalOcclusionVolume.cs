using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Goodies.Portals
{
    public class PortalOcclusionVolume : AgarthanBehaviour
    {
        [EditorReadOnly] public List<AgarthanPortal> OccludedPortals = new();
        [ValidateNull] public Collider Collider;

        protected override void Start()
        {
            base.Start();

            if (OccludedPortals.Count == 0)
                ResolvePortals();
        }

        public static PortalOcclusionVolume GetCurrentVolume(Camera cam)
            => FindObjectsByType<PortalOcclusionVolume>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                .Where(q => q.Collider.bounds.Contains(cam.transform.position))
                .FirstOrDefault();

        public static bool IsInSameVolume(Camera cam, AgarthanPortal portal)
            => FindObjectsByType<PortalOcclusionVolume>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                .Where(q => q.Collider.bounds.Contains(cam.transform.position)
                    && q.OccludedPortals.Contains(portal)).FirstOrDefault() != null;

        [ContextMenu("Resolve visible portals")] public void ResolvePortals()
        {
            OccludedPortals.Clear();
            var portals = FindObjectsByType<AgarthanPortal>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (var portal in portals)
                if (Collider.bounds.Contains(portal.transform.position))
                    OccludedPortals.Add(portal);
        }
    }
}
