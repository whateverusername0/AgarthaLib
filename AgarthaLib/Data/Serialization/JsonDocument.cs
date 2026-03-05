using System;
using System.IO;
using UnityEngine;

namespace AgarthaLib.Data.Serialization
{
    [Serializable] public class JsonDocument<T> : IJsonSerializable<JsonDocument<T>>, ISaveable<JsonDocument<T>>
    {
        public string RelativePath { get; protected set; }
        public string GetFullPath() => Path.Combine(Environment.CurrentDirectory, RelativePath);

        public JsonDocument<T> Deserialize(string json)
            => JsonUtility.FromJson<JsonDocument<T>>(json);

        public string Serialize()
            => JsonUtility.ToJson(this);

        public void Save()
            => File.WriteAllText(GetFullPath(), Serialize());

        public JsonDocument<T> Read()
            => Deserialize(File.ReadAllText(GetFullPath()));
    }
}
