namespace AgarthaLib.Data.Serialization.SerializedTypes
{
    public class SerializedFloat : SerializedNumeric<float>
    {
        public SerializedFloat(float value) : base(value) { }

        public static implicit operator float(SerializedFloat @this)
            => @this.Value;

        public static implicit operator SerializedFloat(float @this)
            => new(@this);
    }
}