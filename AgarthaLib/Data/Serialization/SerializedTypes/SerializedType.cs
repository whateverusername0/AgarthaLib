using System;
using UnityEngine;

namespace AgarthaLib.Data.Serialization.SerializedTypes
{
    [Serializable] public class SerializableType
    {
        [SerializeField] private string _name;
        [SerializeField] private string _assemblyQualifiedName;
        [SerializeField] private string _assemblyName;
        private Type _systemType;

        public string AssemblyName => _assemblyName;
        public string Name => _name;
        public string AssemblyQualifiedName => _assemblyQualifiedName;
        public Type Type => _systemType == null ? GetSystemType() : _systemType;

        private Type GetSystemType()
        {
            _systemType = Type.GetType(_assemblyQualifiedName);
            return _systemType;
        }

        public SerializableType(Type type)
        {
            _systemType = type;
            _name = type.Name;
            _assemblyQualifiedName = type.AssemblyQualifiedName;
            _assemblyName = type.Assembly.FullName;
        }

        public override bool Equals(object obj)
            => obj is SerializableType temp && this.Equals(temp);

        public bool Equals(SerializableType @object)
            => @object.Type.Equals(Type);

        public static bool operator ==(SerializableType a, SerializableType b)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            if ((a is null) || (b is null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(SerializableType a, SerializableType b)
            => !(a == b);

        public override int GetHashCode()
            => base.GetHashCode();
    }
}
