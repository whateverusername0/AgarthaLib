using AgarthaLib.Attributes;
using System;

namespace AgarthaLib.Data.Serialization
{
    [Serializable] public struct SerializableGuid
    {
        public readonly Guid Guid;
        [EditorReadOnly] public string Value;

        public SerializableGuid(Guid guid)
        {
            Guid = guid;
            Value = Guid.ToString();
        }

        public static implicit operator SerializableGuid(Guid g)
            => new(g);

        public static implicit operator Guid(SerializableGuid sg)
            => new(sg.Value);

        public static bool operator ==(SerializableGuid a, SerializableGuid b)
            => a.Guid == b.Guid;

        public static bool operator !=(SerializableGuid a, SerializableGuid b)
            => !(a == b);

        public readonly override bool Equals(object obj)
            => base.Equals(obj);

        public readonly override int GetHashCode()
            => Guid.GetHashCode();
    }
}
