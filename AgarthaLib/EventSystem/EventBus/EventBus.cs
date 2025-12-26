using System;
using System.Collections.Generic;

namespace AgarthaLib.EventSystem.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _subscriptions = new();

        /// <inheritdoc/>
        public void RaiseEvent<TArgs>(TArgs args) where TArgs : class
        {
            var args2 = args; // copy
            if (_subscriptions.TryGetValue(typeof(TArgs), out var handlers))
            {
                foreach (var handler in handlers)
                {
                    var eventHandler = (EventHandlerDelegate<TArgs>)handler;
                    eventHandler?.Invoke(ref args2);
                }
            }
        }

        /// <inheritdoc/>
        public void RaiseEvent<TArgs>(ref TArgs args) where TArgs : class
        {
            if (_subscriptions.TryGetValue(typeof(TArgs), out var handlers))
            {
                foreach (var handler in handlers)
                {
                    var eventHandler = (EventHandlerDelegate<TArgs>)handler;
                    eventHandler?.Invoke(ref args);
                }
            }
        }

        /// <inheritdoc/>
        public void SubscribeEvent<TArgs>(EventHandlerDelegate<TArgs> handler) where TArgs : class
        {
            var key = typeof(TArgs);
            if (_subscriptions.ContainsKey(key) && _subscriptions[key].Contains(handler))
                throw new Exception($"EventBus already has a handler of {handler.GetType()} for event {typeof(TArgs)}");

            _subscriptions[key].Add(handler);
        }

        /// <inheritdoc/>
        public void UnsubscribeEvent<TArgs>(EventHandlerDelegate<TArgs> handler) where TArgs : class
        {
            var key = typeof(TArgs);
            if (!_subscriptions.ContainsKey(key) && !_subscriptions[key].Contains(handler))
                return;

            _subscriptions[key].Remove(handler);
        }
    }
}
