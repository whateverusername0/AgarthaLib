#if USING_NEWTONSOFT_JSON
using AgarthaLib.Assets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Data.JSON
{
    public class UnityObjectJsonConverter<T> : JsonConverter<T> where T : UnityEngine.Object
    {
        public IAssetManifest Manifest;

        public UnityObjectJsonConverter(IAssetManifest manifest)
            => Manifest = manifest;

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            var path = reader.Value as string;
            if (string.IsNullOrWhiteSpace(path)) return null;

            var asset = Manifest.GetAsset<T>(path);
            return asset;
        }

        public override void WriteJson(JsonWriter writer, T value,
            JsonSerializer serializer)
        {
            if (value == null || Manifest == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(Manifest.GetAssetPath(value));
        }
    }
}
#endif