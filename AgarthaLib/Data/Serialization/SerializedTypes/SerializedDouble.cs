using System;

namespace AgarthaLib.Data.Serialization.SerializedTypes
{
    [Serializable] public class SerializedDouble : SerializedNumeric<double>
    {
        public static implicit operator double(SerializedDouble @this)
            => @this.Value;
    }
}