using System;
using UnityEngine;

namespace AgarthaLib.Data.Serialization.SerializedTypes
{
    [Serializable] public class SerializedType
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
            if (_systemType != null)
                return _systemType;

            var type = Type.GetType(_assemblyQualifiedName);
            _systemType = type;
            return type;
        }

        public SerializedType(Type type)
        {
            _systemType = type;
            _name = type.Name;
            _assemblyQualifiedName = type.AssemblyQualifiedName;
            _assemblyName = type.Assembly.FullName;
        }

        public override bool Equals(object obj)
            => obj is SerializedType temp && this.Equals(temp);

        public bool Equals(SerializedType @object)
            => @object.Type.Equals(Type);

        public override int GetHashCode()
            => base.GetHashCode();

        public static bool operator ==(SerializedType a, SerializedType b)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            if ((a is null) || (b is null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(SerializedType a, SerializedType b)
            => !(a == b);

        public static implicit operator Type(SerializedType a)
            => a.GetSystemType();

        public static implicit operator SerializedType(Type a)
            => new(a);
    }
}
