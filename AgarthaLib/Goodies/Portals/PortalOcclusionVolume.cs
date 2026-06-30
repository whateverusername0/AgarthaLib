using AgarthaLib.Attributes;
using AgarthaLib.Collision;
using AgarthaLib.EventSystem;
using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
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

            RenderPortals(false);

            SubscribeEvent<CollisionEnterEvent>(RenderPortals);
            SubscribeEvent<RelayedEvent<CollisionEnterEvent>>(RenderPortals);
            SubscribeEvent<CollisionStayEvent>(RenderPortals);
            SubscribeEvent<RelayedEvent<CollisionStayEvent>>(RenderPortals);

            SubscribeEvent<CollisionExitEvent>(StopRenderingPortals);
            SubscribeEvent<RelayedEvent<CollisionExitEvent>>(StopRenderingPortals);
        }

        public void RenderPortals<T>(GameObject invoker, ref T args)
            => RenderPortals(true);

        public void StopRenderingPortals<T>(GameObject invoker, ref T args)
            => RenderPortals(false);

        public void RenderPortals(bool render)
        {
            if (OccludedPortals == null || OccludedPortals.Count == 0)
                return;

            OccludedPortals.ForEach(q => q.RenderingEnabled = render);
        }

        [ContextMenu("Resolve visible portals")]
        public void ResolvePortals()
        {
            OccludedPortals.Clear();
            var portals = FindObjectsByType<AgarthanPortal>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (var portal in portals)
                if (Collider.bounds.Contains(portal.transform.position))
                    OccludedPortals.Add(portal);
        }
    }
}
