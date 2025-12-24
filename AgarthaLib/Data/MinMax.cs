using System;

namespace Agartha.Data
{
    [Serializable] public struct MinMax
    {
        public float Min, Max;

        public MinMax(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}