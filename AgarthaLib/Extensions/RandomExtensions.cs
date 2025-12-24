using System.Collections.Generic;

namespace Agartha.Extensions
{
    public static class RandomExtensions
    {
        public static T PickRandom<T>(this List<T> list)
        {
            if (list.Count == 0) return default;
            return list[UnityEngine.Random.Range(0, list.Count - 1)];
        }
    }
}