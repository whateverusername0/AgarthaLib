using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.EventSystem
{
    public delegate void EventHandlerDelegate<TInvoker, TArgs>(TInvoker invoker, ref TArgs args)
        where TInvoker : notnull
        where TArgs : notnull;

    public interface IEventBus
    {
        public Dictionary<Type, EventHandlerDelegate<object, object>> GetSubscriptions();

        public void RaiseEvent(GameObject target, object args);
        public void RaiseEvent(GameObject target, ref object args);

        public void RaiseEvent<TEvent>(GameObject target, TEvent args);
        public void RaiseEvent<TEvent>(GameObject target, ref TEvent args);

        public void SubscribeEvent<TArgs>(EventHandlerDelegate<object, TArgs> handler);
        public void UnsubscribeEvent<TArgs>(EventHandlerDelegate<object, TArgs> handler);
    }
}
