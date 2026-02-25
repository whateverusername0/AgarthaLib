using AgarthaLib.EventSystem;
using UnityEngine;

namespace Assets.AgarthaLib.AgarthaLib.EventSystem
{
    [CreateAssetMenu(menuName = "Agartha / Event System / Serialized Event")]
    public class SerializedEvent<T> : ScriptableObject where T : EventBase
    {
        public T Event;
    }
}
