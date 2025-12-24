using AgarthaLib.EventSystem;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Collision
{
    public class BeforeCollisionEnterEvent : TargetedCancellableEvent
    {
        public BeforeCollisionEnterEvent(GameObject target) : base(target) {  }
    }

    public class CollisionEnterEvent : TargetedEventBase
    {
        public CollisionEnterEvent(GameObject target) : base(target) {  }
    }

    public class AfterCollisionEnterEvent : TargetedEventBase
    {
        public AfterCollisionEnterEvent(GameObject target) : base(target) {  }
    }

    public class CollisionStayEvent : TargetedEventBase
    {
        public CollisionStayEvent(GameObject target) : base(target) {  }
    }

    public class CollisionExitEvent : TargetedEventBase
    {
        public CollisionExitEvent(GameObject target) : base(target) { }
    }

    public class CollisionEventTrigger : AgarthanBehaviour
    {
        private void OnCollisionEnter(UnityEngine.Collision collision)
            => BeginCollide(collision.gameObject);

        private void OnCollisionEnter2D(Collision2D collision)
            => BeginCollide(collision.gameObject);

        private void OnTriggerEnter(Collider other)
            => BeginCollide(other.gameObject);

        private void OnTriggerEnter2D(Collider2D other)
            => BeginCollide(other.gameObject);

        private void OnCollisionStay(UnityEngine.Collision collision)
            => CollisionStay(collision.gameObject);

        private void OnCollisionStay2D(Collision2D collision)
            => CollisionStay(collision.gameObject);

        private void OnTriggerStay(Collider other)
            => CollisionStay(other.gameObject);

        private void OnTriggerStay2D(Collider2D collision)
            => CollisionStay(collision.gameObject);

        private void OnCollisionExit(UnityEngine.Collision collision)
            => CollisionExit(collision.gameObject);

        private void OnCollisionExit2D(Collision2D collision)
            => CollisionExit(collision.gameObject);

        private void OnTriggerExit(Collider other)
            => CollisionExit(other.gameObject);

        private void OnTriggerExit2D(Collider2D collision)
            => CollisionExit(collision.gameObject);


        protected virtual void BeginCollide(GameObject other)
        {
            var before = new BeforeCollisionEnterEvent(other);
            RaiseEvent(other, ref before);
            if (before.Cancelled)
                return;

            RaiseEvent(other, new CollisionEnterEvent(other));
            RaiseEvent(other, new AfterCollisionEnterEvent(other));
        }

        protected virtual void CollisionStay(GameObject other)
            => RaiseEvent(other, new CollisionStayEvent(other));

        protected virtual void CollisionExit(GameObject other)
            => RaiseEvent(other, new CollisionExitEvent(other));
    }
}