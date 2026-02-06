using System;
using UnityEngine;

namespace AgarthaLib.Data
{
    [Serializable] public struct MinMax
    {
        public float Min, Max;

        public MinMax(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public static implicit operator MinMaxInt(MinMax m)
            => new(Mathf.RoundToInt(m.Min), Mathf.RoundToInt(m.Max));
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
    }
}