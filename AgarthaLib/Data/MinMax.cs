using System;
using System.Collections.Generic;
using UnityEngine;

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

        public static implicit operator MinMaxInt(MinMax m)
            => new(Mathf.RoundToInt(m.Min), Mathf.RoundToInt(m.Max));

        public static MinMax Zero => new(0, 0);

        public readonly bool IsInBounds(float f) => f >= Min && f <= Max;

        public override readonly string ToString() => $"{Min}-{Max}";
    }

    [Serializable] public struct MinMaxInt
    {
        public int Min, Max;

        public MinMaxInt(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public static implicit operator MinMax(MinMaxInt i)
            => new(i.Min, i.Max);

        public static MinMaxInt Zero => new(0, 0);

        public readonly bool IsInBounds(float f) => f >= Min && f <= Max;

        public override readonly string ToString() => $"{Min}-{Max}";

        public readonly int Length()
        {
            int l = 0;
            for (int i = Min; i < Max; i++) l++;
            return l;
        }
    }
}