namespace AgarthaLib.Data.Serialization.SerializedTypes
{
    public class SerializedDouble : SerializedNumeric<double>
    {
        public SerializedDouble(double value) : base(value) { }

        public static implicit operator double(SerializedDouble @this)
            => @this.Value;

        public static implicit operator SerializedDouble(double @this)
            => new(@this);
    }
}