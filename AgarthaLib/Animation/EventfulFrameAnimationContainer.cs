using AgarthaLib.Data.Serialization.SerializedTypes;
using AgarthaLib.MonoBehavior;
using UnityEngine.Events;

namespace AgarthaLib.Animation
{
    /// <summary>
    ///     Frame animation storage + events.
    /// </summary>
    public abstract class EventfulFrameAnimationContainer<T> : AgarthanBehaviour where T : FrameAnimation<T>
    {
        public T Animation;
        public SerializedDictionary<int, UnityEvent> FrameEvents;
    }
}
