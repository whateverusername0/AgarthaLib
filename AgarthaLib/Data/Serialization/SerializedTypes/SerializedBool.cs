using System;

namespace AgarthaLib.Data.Serialization.SerializedTypes
{
    [Serializable] public class SerializedBool : SerializedProperty<bool>
    {
        public static implicit operator bool(SerializedBool @this)
            => @this.Value;
    }
}