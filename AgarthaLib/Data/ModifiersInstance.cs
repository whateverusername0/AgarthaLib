using System;
using System.Collections.Generic;

namespace AgarthaLib.Data
{
    // TODO finish
    [Serializable] public class ModifiersInstance<T>
    {
        protected readonly Dictionary<string, T> Modifiers;
        protected T DefaultValue;

        public ModifiersInstance()
        {
            Modifiers = new();
            DefaultValue = default;
        }

        public ModifiersInstance(Dictionary<string, T> modifiers) : this()
            => Modifiers = modifiers;

        public ModifiersInstance(Dictionary<string, T> modifiers, T defaultValue) : this(modifiers)
            => DefaultValue = defaultValue;

        public bool ModifierExists(string key)
            => Modifiers.ContainsKey(key);
    }
}
