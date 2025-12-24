using System;

namespace Agartha.WeightedRandom
{
    [Serializable] public class WeightedItem<T>
    {
        public T Value;
        public float Weight;

        public WeightedItem(T val, float weight)
            => (Value, Weight) = (val, weight);
    }
}
