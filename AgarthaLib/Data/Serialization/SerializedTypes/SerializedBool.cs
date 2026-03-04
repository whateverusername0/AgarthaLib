namespace AgarthaLib.Data.Serialization.SerializedTypes
{
    public class SerializedBool : SerializedProperty<bool>
    {
        public SerializedBool(bool value) : base(value) { }

        public static implicit operator bool(SerializedBool @this)
            => @this.Value;

        public static implicit operator SerializedBool(bool @this)
            => new(@this);
    }
}