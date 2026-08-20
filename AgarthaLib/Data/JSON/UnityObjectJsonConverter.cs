#if USING_NEWTONSOFT_JSON
using AgarthaLib.Assets;
using Newtonsoft.Json;
using System;

namespace AgarthaLib.Data.JSON
{
    public class UnityObjectJsonConverter<T> : JsonConverter where T : UnityEngine.Object
    {
        public IAssetManifest Manifest;

        public UnityObjectJsonConverter(IAssetManifest manifest)
            => Manifest = manifest;

        public override bool CanConvert(Type objectType)
        {
            var t = typeof(T);
            var tr = t.IsAssignableFrom(objectType) || objectType.IsAssignableFrom(t);
            return tr;
        }
        //=> typeof(T).IsAssignableFrom(objectType)
        //|| objectType.IsAssignableFrom(typeof(T));

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            var path = reader.Value as string;
            if (string.IsNullOrWhiteSpace(path)) return null;

            var asset = Manifest.GetAsset<T>(path);
            return asset;
        }

        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            if (value == null || Manifest == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(Manifest.GetAssetPath(value as T));
        }
    }
}
#endif