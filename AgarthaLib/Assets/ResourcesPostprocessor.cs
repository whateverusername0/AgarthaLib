#if UNITY_EDITOR && USING_NEWTONSOFT_JSON
using AgarthaLib.Data.Serialization.SerializedTypes;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AgarthaLib.Assets
{
    public class ResourcesPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var allPaths = importedAssets
                .Concat(deletedAssets)
                .Concat(movedAssets)
                .Concat(movedFromAssetPaths);

            var affectedFolders = new HashSet<string>();

            foreach (var path in allPaths)
            {
                if (string.IsNullOrEmpty(path) || path.EndsWith(".meta") || path.Contains("manifest"))
                    continue;

                var resourceFolder = GetResourcesDirectory(path);
                if (!string.IsNullOrEmpty(resourceFolder))
                    affectedFolders.Add(resourceFolder);
            }

            if (affectedFolders.Count == 0) return;

            AssetDatabase.StartAssetEditing();
            try
            {
                foreach (var folderPath in affectedFolders)
                    UpdateManifest(folderPath);
            }
            finally { AssetDatabase.StopAssetEditing(); }
        }

        private static string GetResourcesDirectory(string path)
        {
            var index = path.LastIndexOf("/Resources/");

            if (index != -1)
                return path[..(index + "/Resources".Length)];

            if (path.StartsWith("Assets/Resources"))
                return "Assets/Resources";

            return null;
        }

        private static ResourcesManifest UpdateManifest(string folderPath)
        {
            var manifest = Resources.Load<ResourcesManifest>("manifest");

            if (manifest != null)
            {
                var update = new SerializedDictionary<string, Object>(GetManifestRaw(folderPath));
                if (update != manifest.Manifest)
                    manifest.Manifest = update;
                return manifest;
            }

            manifest = CreateManifest(folderPath);
            return manifest;
        }

        private static ResourcesManifest CreateManifest(string folderPath)
        {
            var inst = ScriptableObject.CreateInstance<ResourcesManifest>();
            inst.Manifest = new(GetManifestRaw(folderPath));

            AssetDatabase.CreateAsset(inst, Path.Combine(folderPath, "manifest.asset"));
            AssetDatabase.SaveAssets();
            return inst;
        }

        public static Dictionary<string, Object> GetManifestRaw(string folderPath)
        {
            var fullPath = GetFullPath(folderPath);
            var paths = new List<string>();
            foreach (var file in GetFilesRecursive(fullPath))
                paths.Add(file);

            var s = Path.DirectorySeparatorChar;
            var trim = Application.dataPath.Replace('/', s);
            trim = Path.Combine(trim, $"Resources{s}");
            for (int i = 0; i < paths.Count; i++)
            {
                // make path relative
                paths[i] = paths[i][trim.Length..];

                // trim file extensions because unity said fuck you
                // you don't have to do this in assetbundles btw (for some reason)
                var ex = paths[i].LastIndexOf('.');
                paths[i] = paths[i][..ex];

                // replace separators because unity said fuck you again
                paths[i] = paths[i].Replace(s, '/');
            }

            return ResourcesManifest.GetManifestRaw(paths);
        }

        private static string GetFullPath(string relativePath)
        {
            var s = Path.DirectorySeparatorChar;
            relativePath = relativePath.Replace('/', s);
            var data = Application.dataPath.Replace('/', s);
            var trim = data[..data.IndexOf($"{s}Assets")];
            return Path.Combine(trim, relativePath);
        }

        private static List<string> GetFilesRecursive(string folderPath)
        {
            var files = Directory.GetFiles(folderPath).ToList();
            files = files.Where(q =>
                    !(string.IsNullOrWhiteSpace(q)
                    || q.EndsWith(".meta")
                    || q.Contains("manifest")))
                .ToList();

            var directories = Directory.GetDirectories(folderPath);
            foreach (var dir in directories)
                files.AddRange(GetFilesRecursive(dir));
            return files;
        }
    }
}
#endif