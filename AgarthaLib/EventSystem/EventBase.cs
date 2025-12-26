using UnityEngine;

namespace AgarthaLib.EventSystem
{
    public class EventBase { }

    public class CancellableEventBase : EventBase
    {
        public bool Cancelled { get; set; }
    }

    public class RelayedEvent<T> where T : class
    {
        public GameObject Source;
        public T Args { get; set; }

        public RelayedEvent(GameObject invoker, T args)
        {
            Source = invoker;
            Args = args;
        }
    }
}