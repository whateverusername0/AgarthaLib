using AgarthaLib.EventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.MonoBehavior
{
    public class AgarthanBehaviour : MonoBehaviour, IEventBus
    {
        private readonly LocalEventBus _bus = new();

        public Dictionary<Type, EventHandlerDelegate<object, object>> GetSubscriptions() => _bus.GetSubscriptions();

        public void RaiseEvent(GameObject target, object args)
            => _bus.RaiseEvent(target, args);

        public void RaiseEvent(GameObject target, ref object args)
            => _bus.RaiseEvent(target, ref args);

        public void RaiseEvent<TEvent>(GameObject target, TEvent args)
            => _bus.RaiseEvent(target, args);

        public void RaiseEvent<TEvent>(GameObject target, ref TEvent args)
            => _bus.RaiseEvent(target, ref args);

        public void SubscribeEvent<TArgs>(EventHandlerDelegate<object, TArgs> handler)
            => _bus.SubscribeEvent(handler);

        public void UnsubscribeEvent<TArgs>(EventHandlerDelegate<object, TArgs> handler)
            => _bus.UnsubscribeEvent(handler);
    }
}
