using UnityEngine;

namespace Agartha.Extensions
{
    public static class Extensions2D
    {
        public static void LookAt2D(this Transform t, Transform target)
            => t.up = t.LookRotation2D(target);

        public static void LookAt2D(this Transform t, Vector3 target)
            => t.up = t.LookRotation2D(target);

        public static Vector2 LookRotation2D(this Transform t, Transform target)
            => (target.position - t.position).normalized;

        public static Vector2 LookRotation2D(this Transform t, Vector3 target)
            => (target - t.position).normalized;
    }
}