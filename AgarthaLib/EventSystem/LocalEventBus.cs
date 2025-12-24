using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.EventSystem
{
    /// <summary>
    ///     Basically every MonoBehaviour is supposed to implement IEventBus.
    ///     See <see cref="AgarthaLib.MonoBehavior.AgarthanBehaviour"/> for an example.
    /// </summary>
    public class LocalEventBus : IEventBus
    {
        private readonly Dictionary<Type, EventHandlerDelegate<object, object>> _subscriptions = new();
        public Dictionary<Type, EventHandlerDelegate<object, object>> GetSubscriptions() => this._subscriptions;

        public void RaiseEvent(GameObject @object, object args)
            => RaiseEvent(@object, ref args);

        public void RaiseEvent(GameObject @object, ref object args)
        {
            // We get all components that implement IEventBus.
            foreach (var bus in @object.GetComponents<Component>().OfType<IEventBus>())
                if (bus.GetSubscriptions().TryGetValue(args.GetType(), out var handler))
                    handler?.Invoke(this, ref args);
        }

        public void RaiseEvent<TEvent>(GameObject target, TEvent args)
            => RaiseEvent(target, ref args);

        public void RaiseEvent<TEvent>(GameObject target, ref TEvent args)
        {
            object boxedArgs = args;
            RaiseEvent(target, boxedArgs);
        }

        public void SubscribeEvent<TArgs>(EventHandlerDelegate<object, TArgs> handler)
        {
            if (_subscriptions.ContainsKey(typeof(TArgs)))
                throw new Exception($"EventBus already has a subscription for event type {typeof(TArgs)}");

            _subscriptions[typeof(TArgs)] = handler as EventHandlerDelegate<object, object>;
        }

        public void UnsubscribeEvent<TArgs>(EventHandlerDelegate<object, TArgs> handler)
        {
            if (!_subscriptions.ContainsKey(typeof(TArgs)))
                return;

            _subscriptions.Remove(typeof(TArgs));
        }
    }
}