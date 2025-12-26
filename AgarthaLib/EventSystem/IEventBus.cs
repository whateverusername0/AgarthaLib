using UnityEngine;

namespace AgarthaLib.EventSystem
{
    public delegate void EventHandlerDelegate<TArgs>(ref TArgs args)
        where TArgs : class;

    public interface IEventBus
    {
        /// <summary>
        ///     Raises an event.
        ///     All subscribers to the event type will receive it.
        /// </summary>
        public void RaiseEvent<TArgs>(TArgs args) where TArgs : class;

        /// <inheritdoc cref="RaiseEvent{TArgs}(GameObject, TArgs)"/>
        public void RaiseEvent<TArgs>(ref TArgs args) where TArgs : class;

        /// <summary>
        ///     Subscribes to an event.
        ///     Once raised, the handler will be invoked.
        /// </summary>
        public void SubscribeEvent<TArgs>(EventHandlerDelegate<TArgs> handler) where TArgs : class;

        /// <summary>
        ///    Unsubscribes from an event.
        ///    It will no longer be invoked.
        /// </summary>
        public void UnsubscribeEvent<TArgs>(EventHandlerDelegate<TArgs> handler) where TArgs : class;
    }
}
