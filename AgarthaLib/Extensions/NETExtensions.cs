using AgarthaLib.Data;
using System.Collections.Generic;

namespace AgarthaLib.Extensions
{
    public static class NETExtensions
    {
        public static float Reverse(this float @object, float max)
            => max - @object;

        public static int Reverse(this int @object, int max)
            => max - @object;

        public static List<T> Reverse<T>(this List<T> @object)
        {
            var list = new List<T>(@object);
            list.Reverse();
            return list;
        }

        public static float Normalize(this float @object, float min, float max)
            => (@object - min) / (max - min);
        public static float Normalize(this float @object, ValueRange<float> thresholds)
            => Normalize(@object, thresholds.Min, thresholds.Max);
    }
}
