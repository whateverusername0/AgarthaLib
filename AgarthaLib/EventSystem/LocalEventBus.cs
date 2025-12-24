using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.EventSystem
{
    public class LocalEventBus : IEventBus
    {
        public readonly Dictionary<Type, EventHandlerDelegate<object, object>> Subscriptions = new();

        public void RaiseEvent(GameObject @object, object args)
            => RaiseEvent(@object, ref args);

        public void RaiseEvent<TEvent>(GameObject @object, TEvent args)
            => RaiseEvent(@object, ref args);

        public void RaiseEvent(GameObject @object, ref object args)
        {
            // We get all components that implement IEventBus.
            foreach (var bus in @object.GetComponents<Component>().OfType<IEventBus>())
                if (Subscriptions.TryGetValue(args.GetType(), out var handler))
                    handler?.Invoke(this, ref args);
        }

        public void RaiseEvent<TEvent>(GameObject @object, ref TEvent args)
            => RaiseEvent(@object, ref args);

        public void SubscribeEvent<TArgs>(EventHandlerDelegate<object, TArgs> handler)
        {
            if (Subscriptions.ContainsKey(typeof(TArgs)))
                throw new Exception($"EventBus already has a subscription for event type {typeof(TArgs)}");

            Subscriptions[typeof(TArgs)] = handler as EventHandlerDelegate<object, object>;
        }

        public void UnsubscribeEvent<TArgs>(EventHandlerDelegate<object, TArgs> handler)
        {
            if (!Subscriptions.ContainsKey(typeof(TArgs)))
                return;

            Subscriptions.Remove(typeof(TArgs));
        }
    }
}