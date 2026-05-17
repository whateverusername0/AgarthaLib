using UnityEngine;

namespace AgarthaLib.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3Int ToInt(this Vector3 a)
            => new(Mathf.RoundToInt(a.x), Mathf.RoundToInt(a.y), Mathf.RoundToInt(a.z));

        public static Vector2Int ToInt(this Vector2 a)
            => new(Mathf.RoundToInt(a.x), Mathf.RoundToInt(a.y));

        public static Vector3 ToFloat(this Vector3Int a)
            => new(a.x, a.y, a.z);

        public static Vector2 ToFloat(this Vector2Int a)
            => new(a.x, a.y);

        public static Vector3 Multiply(this Vector3 a, Vector3 b)
            => new(a.x * b.x, a.y * b.y, a.z * b.z);

        public static Vector2 Multiply(this Vector2 a, Vector2 b)
            => new(a.x * b.x, a.y * b.y);

        public static Vector3 XZ(this Vector3 v)
            => new(v.x, 0, v.z);

        public static Vector3 LookDirection(this Vector3 v, Vector3 target)
            => target - v;

        public static Vector3 Clamp(this Vector3 v, Vector3 min, Vector3 max)
        {
            var x = Mathf.Clamp(v.x, min.x, max.x);
            var y = Mathf.Clamp(v.y, min.y, max.y);
            var z = Mathf.Clamp(v.z, min.z, max.z);
            return new(x, y, z);
        }

        public static Vector3 GetForwardFromEuler(this Vector3 v)
            => new(Mathf.Cos(v.y) * Mathf.Cos(v.x), Mathf.Sin(v.y) * Mathf.Cos(v.x), Mathf.Sin(v.z));
    }
}
