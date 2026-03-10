using UnityEngine;

namespace AgarthaLib.EventSystem
{
    public abstract class EventBase { }

    public abstract class CancellableEventBase : EventBase
    {
        public bool Cancelled { get; set; }
    }

    public class RelayedEvent<T> where T : notnull
    {
        public GameObject Source;
        public T Args { get; set; }

        public RelayedEvent(GameObject invoker, T args)
        {
            Source = invoker;
            Args = args;
        }
    }

    public abstract class PropertyChangedEvent<T> where T : notnull
    {
        public T OldValue, NewValue;
        public PropertyChangedEvent(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}