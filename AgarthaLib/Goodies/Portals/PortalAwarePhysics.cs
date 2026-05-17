using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Goodies.Portals
{
    public static class PortalAwarePhysics
    {
        public static bool RaycastAll(Vector3 position, Vector3 direction, float distance, out List<RaycastHit> result, int mask = -1)
        {
            result = new();
            var rc = Physics.RaycastAll(position, direction, distance, mask);
            if (rc.Length == 0) return false;

            foreach (var hit in rc)
            {
                if (hit.collider.TryGetComponent<AgarthanPortal>(out var p))
                {
                    // ray world bending magic
                    var pos = p.TransformPosition(position);
                    var dir = p.TransformDirection(direction);

                    // subtract the distance so that it won't go forever.
                    var dist = Mathf.Max(distance - Vector3.Distance(hit.collider.transform.position, position), 0);

                    if (RaycastAll(pos, dir, dist, out var newHits, mask))
                        result.AddRange(newHits);

                    break;
                }

                result.Add(hit);
            }

            return result.Count > 0;
        }

        /// <summary>
        ///     A <see cref="Physics.OverlapSphere(Vector3, float, int)"/> variation
        ///     that outputs objects not valid to the `overlapCondition`.
        /// </summary>
        public static bool OverlapSphereUnoccluded(Vector3 position, float radius,
            out List<Collider> hits, Predicate<Collider> overlapCondition, int mask = -1)
        {
            hits = new();

            var sc = Physics.OverlapSphere(position, radius, mask);
            if (sc.Length == 0) return false;

            var cache = new List<Collider>();
            foreach (var hit in sc)
            {
                var direction = hit.transform.position - position;
                var rc = Physics.RaycastAll(position, direction.normalized, radius, mask);
                foreach (var rhit in rc)
                {
                    if (overlapCondition.Invoke(rhit.collider))
                        break;

                    cache.Add(rhit.collider);
                }
            }

            hits = cache.Distinct().ToList();
            return true;
        }

        public static bool OverlapHemisphereUnoccluded(Vector3 position, Vector3 forward, float radius,
            out List<Collider> hits, Predicate<Collider> overlapCondition, int mask = -1)
        {
            hits = new();
            if (!OverlapSphereUnoccluded(position, radius, out hits, overlapCondition, mask))
                return false;

            // if Vector3.Dot() <= 0 -> object is behind the "half sphere" thus invalid.
            hits = hits.Where(q => Vector3.Dot((q.transform.position - position).normalized, forward) > 0)
                .Distinct().ToList();

            return true;
        }
    }
}
