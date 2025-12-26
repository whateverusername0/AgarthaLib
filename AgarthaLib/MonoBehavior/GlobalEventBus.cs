using AgarthaLib.EventSystem;
using AgarthaLib.EventSystem.EventBus;

namespace AgarthaLib.MonoBehavior
{
    public class GlobalEventBus : AgarthanSingleton<GlobalEventBus>, IEventBus
    {
        private readonly EventBus _bus = new();

        public void RaiseEvent<TArgs>(TArgs args) where TArgs : class
            => _bus.RaiseEvent(args);

        public void RaiseEvent<TArgs>(ref TArgs args) where TArgs : class
            => _bus.RaiseEvent(ref args);

        public void SubscribeEvent<TArgs>(EventHandlerDelegate<TArgs> handler) where TArgs : class
            => _bus.SubscribeEvent(handler);

        public void UnsubscribeEvent<TArgs>(EventHandlerDelegate<TArgs> handler) where TArgs : class
            => _bus.UnsubscribeEvent(handler);
    }
}
