using AgarthaLib.Goodies.Portals;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Extensions
{
    public static class Physics
    {
        public static bool RaycastPortalAware(Vector3 position, Vector3 direction, float distance, out RaycastHit[] result, int mask = -1)
        {
            var hits = new List<RaycastHit>();
            result = hits.ToArray();
            var rc = UnityEngine.Physics.RaycastAll(position, direction, distance, mask);
            if (rc.Length == 0) return false;

            foreach (var hit in result)
            {
                if (hit.collider.TryGetComponent<AgarthanPortal>(out var p))
                {
                    // ray world bending magic
                    var pos = AgarthanPortal.TransformPosition(p, p.LinkedPortal, position);
                    var dir = AgarthanPortal.TransformDirection(p, p.LinkedPortal, direction);

                    // subtract the distance so that it won't go forever.
                    var dist = Mathf.Max(distance - Vector3.Distance(hit.collider.transform.position, position), 0);

                    if (RaycastPortalAware(pos, dir, dist, out var newHits, mask))
                        hits.AddRange(newHits);

                    break;
                }

                hits.Add(hit);
            }

            return true;
        }

        /// <summary>
        ///     A <see cref="UnityEngine.Physics.OverlapSphere(Vector3, float, int)"/> variation
        ///     that outputs objects not valid to the `overlapCondition`.
        /// </summary>
        public static bool OverlapSphereUnoccluded(Vector3 position, float radius, out Collider[] hits, Predicate<Collider> overlapCondition, int mask = -1)
        {
            hits = new Collider[0];

            var sc = UnityEngine.Physics.OverlapSphere(position, radius, mask);
            if (sc.Length == 0) return false;

            var cache = new List<Collider>();
            foreach (var hit in sc)
            {
                var direction = hit.transform.position - position;
                var rc = UnityEngine.Physics.RaycastAll(position, direction.normalized, radius, mask);
                foreach (var rhit in rc)
                {
                    if (overlapCondition.Invoke(rhit.collider))
                        break;

                    cache.Add(rhit.collider);
                }
            }

            hits = cache.ToArray();
            return true;
        }
    }
}
