using Agartha.Data;
using System;

namespace Agartha.Extensions
{
    public static class NETExtensions
    {
        public static float Reverse(this float @object, float max)
        {
            return max - @object;
        }

        public static float Normalize(this float @object, float min, float max)
            => (@object - min) / (max - min);
        public static float Normalize(this float @object, MinMax thresholds)
            => Normalize(@object, thresholds.Min, thresholds.Max);
    }
}
