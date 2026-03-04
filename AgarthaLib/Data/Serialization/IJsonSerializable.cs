namespace AgarthaLib.Data.Serialization
{
    public interface IJsonSerializable<T>
    {
        public T Deserialize(string json);
        public string Serialize();
    }
}
