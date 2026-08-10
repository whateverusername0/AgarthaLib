#if USING_NEWTONSOFT_JSON
using AgarthaLib.Data.Serialization.SerializedTypes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Assets
{
    public class ResourcesManifest : ScriptableObject, IAssetManifest
    {
        [SerializeField] public SerializedDictionary<string, Object> Manifest = new();

        // unity can't serialize a list properly
        public string Serialize()
            => JsonConvert.SerializeObject(Manifest.Keys);

        public T GetAsset<T>(string path) where T : Object
        {
            var value = Manifest[path];
            if (value == null) value = Resources.Load<T>(path);
            return value as T;
        }

        public bool TryGetAsset<T>(string path, out T obj) where T : Object
        {
            obj = GetAsset<T>(path);
            return obj != null;
        }

        public string GetAssetPath(Object asset)
            => Manifest.FirstOrDefault(q => q.Value == asset).Key;

        public bool TryGetAssetPath(Object asset, out string path)
        {
            path = GetAssetPath(asset);
            return !string.IsNullOrEmpty(path);
        }

        #region Static

        public static ResourcesManifest LoadFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            var paths = JsonConvert.DeserializeObject<List<string>>(text);
            if (paths == null || paths.Count == 0) return null;

            var inst = CreateInstance<ResourcesManifest>();
            inst.Manifest = new(GetManifestRaw(paths));
            return inst;
        }

        public static Dictionary<string, Object> GetManifestRaw(List<string> paths)
        {
            var manifest = new Dictionary<string, Object>();
            foreach (var path in paths)
            {
                var resource = Resources.Load(path);
                if (resource == null) continue;
                manifest.Add(path, resource);
            }
            return manifest;
        }

        #endregion
    }
}
#endif