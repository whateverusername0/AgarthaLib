using AgarthaLib.EventSystem;
using UnityEngine.SceneManagement;

namespace AgarthaLib.Scenes
{
    public class ActiveSceneChangedGlobalEvent : EventBase
    {
        public Scene Current, Next;

        public ActiveSceneChangedGlobalEvent(Scene current, Scene next)
        {
            Current = current;
            Next = next;
        }
    }

    public class SceneLoadedGlobalEvent : EventBase
    {
        public Scene Scene;
        public LoadSceneMode Mode;

        public SceneLoadedGlobalEvent(Scene scene, LoadSceneMode mode)
        {
            Scene = scene;
            Mode = mode;
        }
    }

    public class SceneUnloadedGlobalEvent : EventBase
    {
        public Scene Scene;

        public SceneUnloadedGlobalEvent(Scene scene)
            => Scene = scene;
    }
}
