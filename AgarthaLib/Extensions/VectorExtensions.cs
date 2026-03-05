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
    }
}
