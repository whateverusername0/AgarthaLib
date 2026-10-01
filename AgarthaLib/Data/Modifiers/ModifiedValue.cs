using AgarthaLib.Attributes;
using AgarthaLib.Data.Serialization.SerializedTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Data.Modifiers
{
    [Serializable] public class ModifiedValue<TValue>
    {
        [SerializeField, EditorReadOnly]
        protected SerializedDictionary<Type, IValueModifier<TValue>> _modifiers = new();

        public TValue BaseValue;

        private Func<TValue, TValue, TValue> _modifierFunction;

        public TValue Value
        {
            get
            {
                var value = BaseValue;
                foreach (var mod in _modifiers.Values)
                    value = _modifierFunction.Invoke(value, mod.Value);
                return value;
            }
        }

        public Dictionary<Type, IValueModifier<TValue>> Modifiers => _modifiers.Dictionary;

        public ModifiedValue(TValue baseValue, Func<TValue, TValue, TValue> getValueFunc)
        {
            BaseValue = baseValue;
        }

        public static implicit operator TValue(ModifiedValue<TValue> mv)
            => mv.BaseValue;

        public void AddModifier(Type t, IValueModifier<TValue> mod)
        {
            if (_modifiers.ContainsKey(t))
            {
                _modifiers[t] = mod;
                return;
            }
            _modifiers.Add(t, mod);
        }

        public void AddModifier(Type t, TValue mod)
        {
            var svm = new StaticValueModifier<TValue>(mod);
            if (_modifiers.ContainsKey(t))
            {
                _modifiers[t] = svm;
                return;
            }
            _modifiers.Add(t, svm);
        }

        public void RemoveModifier(Type t)
        {
            if (!_modifiers.ContainsKey(t)) return;
            _modifiers.Remove(t);
        }
    }
}