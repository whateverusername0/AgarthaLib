using AgarthaLib.EventSystem;
using UnityEngine;

namespace AgarthaLib.MonoBehavior
{
    public class AgarthanBehaviour : MonoBehaviour, IEventBus
    {
        private readonly LocalEventBus _bus = new();

        public void RaiseEvent(GameObject @object, object args)
            => _bus.RaiseEvent(@object, args);

        public void RaiseEvent<TEvent>(GameObject @object, TEvent args)
            => _bus.RaiseEvent(@object, args);

        public void RaiseEvent(GameObject @object, ref object args)
            => _bus.RaiseEvent(@object, ref args);

        public void RaiseEvent<TEvent>(GameObject @object, ref TEvent args)
            => _bus.RaiseEvent(@object, ref args);

        public void SubscribeEvent<TArgs>(EventHandlerDelegate<object, TArgs> handler)
            => _bus.SubscribeEvent(handler);

        public void UnsubscribeEvent<TArgs>(EventHandlerDelegate<object, TArgs> handler)
            => _bus.UnsubscribeEvent(handler);
    }
}
