using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.EventSystem.EventBus
{
    /// <summary>
    ///     Basically every MonoBehaviour is supposed to implement <see cref="ILocalEventBus"/>.
    ///     See <see cref="MonoBehavior.AgarthanBehaviour"/> for an example.
    /// </summary>
    public class LocalEventBus : ILocalEventBus
    {
        private readonly Dictionary<Type, Delegate> _subscriptions = new();
        public Dictionary<Type, Delegate> GetSubscriptions() => new(_subscriptions); // copy

        /// <inheritdoc/>
        public void RaiseEvent<TArgs>(GameObject @object, TArgs args) where TArgs : class
        {
            var args2 = args; // copy
            foreach (var bus in @object.GetComponents<Component>().OfType<ILocalEventBus>())
            {
                if (bus.GetSubscriptions().TryGetValue(args2.GetType(), out var handler))
                {
                    var eventHandler = (EventHandlerDelegate<TArgs>)handler;
                    eventHandler?.Invoke(this, ref args2);
                }
            }
        }

        /// <inheritdoc/>
        public void RaiseEvent<TArgs>(GameObject @object, ref TArgs args) where TArgs : class
        {
            foreach (var bus in @object.GetComponents<Component>().OfType<ILocalEventBus>())
            {
                if (bus.GetSubscriptions().TryGetValue(args.GetType(), out var handler))
                {
                    var eventHandler = (EventHandlerDelegate<TArgs>)handler;
                    eventHandler?.Invoke(this, ref args);
                }
            }
        }

        /// <inheritdoc/>
        public void SubscribeEvent<TArgs>(EventHandlerDelegate<TArgs> handler)
        {
            if (_subscriptions.ContainsKey(typeof(TArgs)))
                throw new Exception($"EventBus already has a subscription for event type {typeof(TArgs)}");

            _subscriptions[typeof(TArgs)] = handler;
        }

        /// <inheritdoc/>
        public void UnsubscribeEvent<TArgs>(EventHandlerDelegate<TArgs> handler)
        {
            if (!_subscriptions.ContainsKey(typeof(TArgs)))
                return;

            _subscriptions.Remove(typeof(TArgs));
        }
    }
}