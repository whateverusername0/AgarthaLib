using System;

namespace AgarthaLib.Data.Serialization.SerializedTypes
{
    [Serializable] public class SerializedFloat : SerializedNumeric<float>
    {
        public static implicit operator float(SerializedFloat @this)
            => @this.Value;
    }
}