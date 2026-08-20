#if USING_NEWTONSOFT_JSON
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Data.JSON
{
    public class ScriptableObjectJsonConverter<T> : JsonConverter<T> where T : ScriptableObject
    {
        public override T ReadJson(JsonReader reader, Type objectType, T existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            var jo = JObject.Load(reader);
            using var subreader = jo.CreateReader();

            var inst = ScriptableObject.CreateInstance<T>();
            serializer.Populate(subreader, inst);

            return inst;
        }

        public override void WriteJson(JsonWriter writer, T value,
            JsonSerializer serializer)
        {
            // serializing like a normal class because instanceId sucks!
            var t = JToken.FromObject(value);
            t.WriteTo(writer, serializer.Converters.ToArray());
        }
    }
}
#endif