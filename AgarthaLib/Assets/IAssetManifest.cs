using UnityEngine;

namespace AgarthaLib.Assets
{
    public interface IAssetManifest
    {
        T GetAsset<T>(string path) where T : Object;
        bool TryGetAsset<T>(string path, out T obj) where T : Object;
    }
}
