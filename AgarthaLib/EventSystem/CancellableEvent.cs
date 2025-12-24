using System.Linq;

namespace Agartha.EventSystem
{
    /// <summary>
    ///     A generic event that can be cancelled.
    /// </summary>
    public class CancellableEvent : EventBase
    {
        public bool Cancelled { get; set; } = false;

        /// <summary>
        ///     Use this if you wanna make it more readable.
        /// </summary>
        public static CancellableEvent Cancel = new(true);

        // to state if the event's been cancelled or not you just make a new class instance.
        // it's ass.
        public CancellableEvent(bool cancelled) : base()
            => Cancelled = cancelled;

        public static implicit operator CancellableEvent(bool cancelled)
            => new(cancelled);
    }

    public delegate CancellableEvent CancellableEventDelegate();
    public delegate CancellableEvent CancellableEventDelegate<A, B>(A invoker, B target);

    public static class CancellableEventExtensions
    {
        public static bool IsCancelled(this CancellableEventDelegate args)
            => (args == null) || args.GetInvocationList().Select(q => (CancellableEvent)q.DynamicInvoke(null)).Any(q => q.Cancelled);

        public static bool IsCancelled<A, B>(this CancellableEventDelegate<A, B> args, A invoker, B target)
            => (args == null) || args.GetInvocationList().Select(q => (CancellableEvent)q.DynamicInvoke(invoker, target)).Any(q => q.Cancelled);
    }
}