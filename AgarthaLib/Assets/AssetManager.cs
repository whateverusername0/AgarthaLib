using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Assets
{
    public class AssetManager : AgarthanSingleton<AssetManager>, IAssetManifest
    {
        protected List<IAssetManifest> Manifests = new();

        protected override void Start()
        {
            base.Start();
            ResolveManifests();
        }

        public void ResolveManifests()
        {
            Manifests.Clear();

            // this should en theorem run through all of them
            var resourceManifests = Resources.LoadAll<ResourcesManifest>("manifest");
            Manifests.AddRange(resourceManifests);

            var bundles = AssetBundle.GetAllLoadedAssetBundles();
            foreach (var bundle in bundles)
            {
                var abManifests = bundle.LoadAllAssets<ResourcesManifest>();
                Manifests.AddRange(abManifests);
            }
        }

        public List<T> GetAllAssets<T>(string path) where T : Object
        {
            if (Manifests.Count == 0) ResolveManifests();
            var l = new List<T>();
            foreach (var manifest in Manifests)
            {
                var assets = manifest.GetAllAssets<T>(path);
                if (assets != null) l.AddRange(assets);
            }
            return l;
        }

        public T GetAsset<T>(string path) where T : Object
        {
            if (Manifests.Count == 0) ResolveManifests();

            foreach (var manifest in Manifests)
            {
                var asset = manifest.GetAsset<T>(path);
                if (asset != null) return asset;
            }
            return null;
        }

        public bool TryGetAsset<T>(string path, out T obj) where T : Object
        {
            obj = GetAsset<T>(path);
            return obj != null;
        }

        public string GetAssetPath(Object asset)
        {
            if (Manifests.Count == 0) ResolveManifests();

            foreach (var manifest in Manifests)
            {
                var path = manifest.GetAssetPath(asset);
                if (!string.IsNullOrWhiteSpace(path))
                    return path;
            }
            return null;
        }

        public bool TryGetAssetPath(Object asset, out string path)
        {
            path = GetAssetPath(asset);
            return !string.IsNullOrWhiteSpace(path);
        }
    }
}
