using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.EventSystem
{
    public delegate void LocalEventHandlerDelegate<TArgs>(GameObject invoker, ref TArgs args)
        where TArgs : class;

    public interface ILocalEventBus
    {
        public Dictionary<Type, Delegate> GetSubscriptions();

        /// <summary>
        ///     Raises an event to a target's <see cref="GameObject"/>.
        ///     All <see cref="MonoBehaviour"/>s on such <see cref="GameObject"/> that implement <see cref="ILocalEventBus"/>
        ///     will receive the event if they have a subscription for the event type.
        /// </summary>
        /// <param name="target">Target <see cref="GameObject"/></param>
        public void RaiseEvent<TArgs>(GameObject invoker, GameObject target, TArgs args) where TArgs : class;

        /// <inheritdoc cref="RaiseEvent{TArgs}(GameObject, TArgs)"/>
        public void RaiseEvent<TArgs>(GameObject invoker, GameObject target, ref TArgs args) where TArgs : class;

        /// <summary>
        ///     Subscribes to an event.
        ///     Once raised, the handler will be invoked.
        /// </summary>
        public void SubscribeEvent<TArgs>(LocalEventHandlerDelegate<TArgs> handler) where TArgs : class;

        /// <summary>
        ///    Unsubscribes from an event.
        ///    It will no longer be invoked.
        /// </summary>
        public void UnsubscribeEvent<TArgs>(LocalEventHandlerDelegate<TArgs> handler) where TArgs : class;
    }
}
