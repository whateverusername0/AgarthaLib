using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.EventSystem
{
    public delegate void EventHandlerDelegate<TArgs>(object invoker, ref TArgs args);

    public interface ILocalEventBus
    {
        public Dictionary<Type, Delegate> GetSubscriptions();

        /// <summary>
        ///     Raises an event to a target's <see cref="GameObject"/>.
        ///     All <see cref="MonoBehaviour"/>s on such <see cref="GameObject"/> that implement <see cref="ILocalEventBus"/>
        ///     will receive the event if they have a subscription for the event type.
        /// </summary>
        /// <param name="target">Target <see cref="GameObject"/></param>
        public void RaiseEvent<TArgs>(GameObject target, TArgs args) where TArgs : class;

        /// <inheritdoc cref="RaiseEvent{TArgs}(GameObject, TArgs)"/>
        public void RaiseEvent<TArgs>(GameObject target, ref TArgs args) where TArgs : class;

        /// <summary>
        ///     Subscribes to an event.
        ///     Once raised, the handler will be invoked.
        /// </summary>
        public void SubscribeEvent<TArgs>(EventHandlerDelegate<TArgs> handler);

        /// <summary>
        ///    Unsubscribes from an event.
        ///    It will no longer be invoked.
        /// </summary>
        public void UnsubscribeEvent<TArgs>(EventHandlerDelegate<TArgs> handler);
    }
}
