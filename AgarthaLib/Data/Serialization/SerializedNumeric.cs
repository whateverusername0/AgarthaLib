using System;
using UnityEngine;

namespace AgarthaLib.Data.Serialization
{
    // TODO: Change to INumber?
    [Serializable] public abstract class SerializedNumeric<T> : SerializedProperty<T> where T : struct, IComparable<T>
    {
        [SerializeField] protected ValueRange<T> _range;

        public T Min => _range.Min;
        public T Max => _range.Max;

        public override T Value
        {
            get => _value;
            set => _value = Clamp(value);
        }

        public static implicit operator T(SerializedNumeric<T> @this)
            => @this.Value;

        public T Clamp(T value)
        {
            if (value.CompareTo(Min) < 0)
                return Min;

            if (value.CompareTo(Max) > 0)
                return Max;

            return value;
        }
    }
}
