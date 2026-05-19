using System;

namespace AgarthaLib.Data.Serialization.SerializedTypes
{
    [Serializable] public class SerializedInt : SerializedNumeric<int>
    {
        public static implicit operator int(SerializedInt @this)
            => @this.Value;
    }
}