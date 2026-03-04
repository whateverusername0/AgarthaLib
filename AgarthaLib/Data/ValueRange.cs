using System;

namespace AgarthaLib.Data
{
    [Serializable] public struct ValueRange<T> where T : IComparable<T>
    {
        public T Min, Max;

        public ValueRange(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public override readonly string ToString() => $"{Min}-{Max}";
    }
}