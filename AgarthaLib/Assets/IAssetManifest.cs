using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Assets
{
    public interface IAssetManifest
    {
        List<T> GetAllAssets<T>(string path) where T : Object;
        T GetAsset<T>(string path) where T : Object;
        bool TryGetAsset<T>(string path, out T obj) where T : Object;
        string GetAssetPath(Object asset);
        bool TryGetAssetPath(Object asset, out string path);
    }
}
