using AgarthaLib.Data.Serialization.SerializedTypes;
using AgarthaLib.MonoBehavior;
using UnityEngine;
using UnityEngine.Events;

namespace AgarthaLib.Animation
{
    /// <summary>
    ///     Frame animation storage + events.
    /// </summary>
    /// <typeparam name="TAnim"> A concrete frame animation class. </typeparam>
    /// <typeparam name="TFrame"> A concrete frame type. </typeparam>
    public abstract class EventfulFrameAnimationContainer<TAnim, TFrame> : AgarthanBehaviour
        where TAnim : FrameAnimation<TFrame>
        where TFrame : Object
    {
        public TAnim Animation;
        public SerializedDictionary<int, UnityEvent> FrameEvents;
    }
}
