namespace AgarthaLib.EventSystem
{
    /// <summary>
    ///     A generic class for an Event.
    /// </summary>
    public abstract class EventBase { }

    public delegate void EventDelegate();

    public delegate void EventDelegate<A, B>(A invoker, B target);
}