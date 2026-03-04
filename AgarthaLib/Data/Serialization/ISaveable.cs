namespace AgarthaLib.Data.Serialization
{
    public interface ISaveable<T>
    {
        public string GetFullPath();
        public void Save();
        public T Read();
    }
}
