namespace AgarthaLib.Data.Serialization.SerializedTypes
{
    public class SerializedInt : SerializedNumeric<int>
    {
        public SerializedInt(int value) : base(value) { }

        public static implicit operator int(SerializedInt @this)
            => @this.Value;

        public static implicit operator SerializedInt(int @this)
            => new(@this);
    }
}