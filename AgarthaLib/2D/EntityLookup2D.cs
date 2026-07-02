using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib._2D
{
    public static class EntityLookup2D
    {
        public static Transform FindClosest(Transform origin, float radius, Func<Collider2D, bool> predicate)
        {
            var eligible = GetAllEligible(origin, radius, predicate);
            return FindClosest(origin, eligible);
        }

        public static Transform FindClosest(Transform origin, List<Transform> entities)
        {
            Transform closest = null;
            if (entities == null || entities.Count == 0) return null;

            foreach (var e in entities)
            {
                if (closest == null)
                {
                    closest = e;
                    continue;
                }

                var distance = Vector2.Distance(origin.position, e.transform.position);
                var closestDistance = Vector2.Distance(origin.position, closest.position);

                var rc = Physics2D.RaycastAll(origin.position, (e.transform.position - origin.position).normalized, distance);

                if (distance < closestDistance && rc.Length <= 1)
                    closest = e.transform;
            }

            return closest;
        }

        public static List<Transform> GetAllEligible(Transform origin, float radius, Func<Collider2D, bool> predicate)
        {
            var sc = Physics2D.OverlapCircleAll(origin.position, radius);
            if (sc.Length == 0) return null;

            var eligible = sc.Where(predicate).ToList();
            if (eligible.Count == 0) return null;

            return eligible.Select(q => q.transform).ToList();
        }
    }
}