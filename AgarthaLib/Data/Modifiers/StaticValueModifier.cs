using System;
using UnityEngine;

namespace AgarthaLib.Data.Modifiers
{
    [Serializable]
    public class StaticValueModifier<T> : IValueModifier<T>
    {
        [SerializeField] private T _value;

        public StaticValueModifier(T value)
            => _value = value;

        public T Value => _value;
    }
}
