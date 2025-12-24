using UnityEngine;

namespace AgarthaLib.EventSystem
{
    public delegate void EventHandlerDelegate<TInvoker, TArgs>(TInvoker invoker, ref TArgs args)
        where TInvoker : notnull
        where TArgs : notnull;

    public interface IEventBus
    {
        public void RaiseEvent(GameObject @object, object args);
        public void RaiseEvent<TEvent>(GameObject @object, TEvent args);

        public void RaiseEvent(GameObject @object, ref object args);
        public void RaiseEvent<TEvent>(GameObject @object, ref TEvent args);
        
        public void SubscribeEvent<TArgs>(EventHandlerDelegate<object, TArgs> handler);
        public void UnsubscribeEvent<TArgs>(EventHandlerDelegate<object, TArgs> handler);
    }
}
