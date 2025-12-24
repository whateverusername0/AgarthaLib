using UnityEngine;

namespace AgarthaLib.EventSystem
{
    public class EventBase { }

    public class TargetedEventBase
    {
        public GameObject Target;

        public TargetedEventBase(GameObject target) { Target = target; }
    }

    public class CancellableEventBase : EventBase
    {
        public bool Cancelled { get; set; }
    }

    public class TargetedCancellableEvent : TargetedEventBase
    {
        public bool Cancelled { get; set; }

        public TargetedCancellableEvent(GameObject target) : base(target) { }
    }
}
