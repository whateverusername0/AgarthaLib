using AgarthaLib.EventSystem;
using AgarthaLib.EventSystem.EventBus;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.MonoBehavior
{
    /// <summary>
    ///     Base class for all <see cref="MonoBehaviour"/> objects that want to use AgarthaLib's functionality.
    ///     Sure you can manually implement everything, but you'd be better off inheriting this bad boy.
    /// </summary>
    public class AgarthanBehaviour : MonoBehaviour, ILocalEventBus
    {
        #region Events

        private readonly LocalEventBus _bus = new();

        /// <inheritdoc cref="LocalEventBus.GetSubscriptions"/>
        public Dictionary<Type, Delegate> GetSubscriptions()
            => _bus.GetSubscriptions();

        /// <inheritdoc/>
        public void RaiseEvent<TArgs>(GameObject target, TArgs args) where TArgs : class
            => _bus.RaiseEvent(target, args);

        /// <inheritdoc/>
        public void RaiseEvent<TArgs>(GameObject target, ref TArgs args) where TArgs : class
            => _bus.RaiseEvent(target, ref args);

        /// <inheritdoc/>
        public void SubscribeEvent<TArgs>(EventHandlerDelegate<TArgs> handler)
            => _bus.SubscribeEvent(handler);

        /// <inheritdoc/>
        public void UnsubscribeEvent<TArgs>(EventHandlerDelegate<TArgs> handler)
            => _bus.UnsubscribeEvent(handler);

        #endregion
    }
}
