using System.IO;
using UnityEngine;

namespace AgarthaLib.IO
{
    public static class UnityPath
    {
        public static string MakeSystemNative(string path)
            => path.Replace('/', Path.DirectorySeparatorChar);

        public static string MakeUnityNative(string path)
            => path.Replace(Path.DirectorySeparatorChar, '/');

        public static string ApplicationDataPath()
            => MakeSystemNative(Application.dataPath);

        public static string ConsoleLogPath()
            => MakeSystemNative(Application.consoleLogPath);

        public static string PersistentDataPath()
            => MakeSystemNative(Application.persistentDataPath);

        public static string StreamingAssetsPath()
            => MakeSystemNative(Application.streamingAssetsPath);

        public static string TemporaryCachePath()
            => MakeSystemNative(Application.temporaryCachePath);
    }
}
