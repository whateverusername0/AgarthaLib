using AgarthaLib.EventSystem;
using UnityEngine;

namespace AgarthaLib.Collision
{
    public class CollisionEventTrigger : MonoBehaviour
    {
        public CancellableEventDelegate<CollisionEventTrigger, GameObject> BeforeCollisionEnterEvent;
        public EventDelegate<CollisionEventTrigger, GameObject> CollisionEnterEvent;
        public EventDelegate<CollisionEventTrigger, GameObject> AfterCollisionEnterEvent;

        public EventDelegate<CollisionEventTrigger, GameObject> CollisionStayEvent;
        public EventDelegate<CollisionEventTrigger, GameObject> CollisionExitEvent;

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
            if (BeforeCollisionEnterEvent != null && (bool)BeforeCollisionEnterEvent?.IsCancelled(this, other))
                return;

            CollisionEnterEvent?.Invoke(this, other);
            AfterCollisionEnterEvent?.Invoke(this, other);
        }

        protected virtual void CollisionStay(GameObject other)
            => CollisionStayEvent?.Invoke(this, other);

        protected virtual void CollisionExit(GameObject other)
            => CollisionExitEvent?.Invoke(this, other);
    }
}