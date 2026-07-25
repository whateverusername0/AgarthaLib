using AgarthaLib.MonoBehavior;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AgarthaLib.Scenes
{
    public class AgarthanSceneManager : AgarthanSingleton<AgarthanSceneManager>
    {
        protected override void Awake()
        {
            base.Awake();

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnActiveSceneChanged(Scene current, Scene next)
            => RaiseGlobalEvent(new ActiveSceneChangedGlobalEvent(current, next));

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
            => RaiseGlobalEvent(new SceneLoadedGlobalEvent(scene, mode));

        private void OnSceneUnloaded(Scene scene)
            => RaiseGlobalEvent(new SceneUnloadedGlobalEvent(scene));

        public void LoadScene(Scene scene, LoadSceneMode mode = LoadSceneMode.Single)
            => SceneManager.LoadScene(scene.name, mode);

        public void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
            => SceneManager.LoadScene(name, mode);

        public void RestartScene()
            => LoadScene(SceneManager.GetActiveScene());

        [Obsolete("Preferably use UnloadSceneAsync instead.")]
        public void UnloadSceneUnsafe(Scene scene)
            => SceneManager.UnloadScene(scene.name);

        public AsyncOperation UnloadSceneAsync(Scene scene)
            => SceneManager.UnloadSceneAsync(scene.name);

        public bool TryLoadSceneFromBundle(AssetBundle bundle, int index = 0,
            LoadSceneMode mode = LoadSceneMode.Single)
        {
            var scenes = bundle.GetAllScenePaths();
            if (scenes.Length == 0 || scenes.Length < index)
                return false;

            LoadScene(scenes[index], mode);
            return true;
        }
    }
}
