using AgarthaLib.Collision.Handlers;
using AgarthaLib.EventSystem;
using AgarthaLib.MonoBehavior;
using UnityEngine;
using UnityEngine.Events;

namespace AgarthaLib.Collision
{
    public class CollisionEventTrigger : AgarthanBehaviour
    {
        [SerializeField] private UnityEvent _collisionEnter;
        [SerializeField] private UnityEvent _collisionStay;
        [SerializeField] private UnityEvent _collisionExit;

        protected override void Start()
        {
            base.Start();

            SubscribeEvent<RelayedEvent<CollisionEnterEvent>>(OnRelayedCollisionEnterEvent);
            SubscribeEvent<RelayedEvent<Collision2DEnterEvent>>(OnRelayedCollisionEnterEvent);
            SubscribeEvent<RelayedEvent<CollisionStayEvent>>(OnRelayedCollisionStayEvent);
            SubscribeEvent<RelayedEvent<Collision2DStayEvent>>(OnRelayedCollisionStayEvent);
            SubscribeEvent<RelayedEvent<CollisionExitEvent>>(OnRelayedCollisionExitEvent);
            SubscribeEvent<RelayedEvent<Collision2DExitEvent>>(OnRelayedCollisionExitEvent);
        }

        private void OnCollisionEnter(UnityEngine.Collision other)
            => CollisionEnter(other.collider);

        private void OnCollisionEnter2D(Collision2D other)
            => CollisionEnter2D(other.collider);

        private void OnTriggerEnter(Collider other)
            => CollisionEnter(other);

        private void OnTriggerEnter2D(Collider2D other)
            => CollisionEnter2D(other);

        private void OnCollisionStay(UnityEngine.Collision other)
            => CollisionStay(other.collider);

        private void OnCollisionStay2D(Collision2D other)
            => CollisionStay2D(other.collider);

        private void OnTriggerStay(Collider other)
            => CollisionStay(other);

        private void OnTriggerStay2D(Collider2D other)
            => CollisionStay2D(other);

        private void OnCollisionExit(UnityEngine.Collision other)
            => CollisionExit(other.collider);

        private void OnCollisionExit2D(Collision2D other)
            => CollisionExit2D(other.collider);

        private void OnTriggerExit(Collider other)
            => CollisionExit(other);

        private void OnTriggerExit2D(Collider2D other)
            => CollisionExit2D(other);

        private bool CheckCollisionLink(bool expected)
            => TryGetComponent<CollisionEventLink>(out var link) && link.CollidingGlobal == expected;

        protected void CollisionEnter(Collider other)
        {
            if (CheckCollisionLink(true))
                return;

            var before = new BeforeCollisionEnterEvent(other);
            RaiseEvent(gameObject, ref before);
            if (before.Cancelled)
                return;

            RaiseEvent(gameObject, new CollisionEnterEvent(other));
            _collisionEnter?.Invoke();
            RaiseEvent(gameObject, new AfterCollisionEnterEvent(other));
        }

        protected void CollisionEnter2D(Collider2D other)
        {
            if (CheckCollisionLink(true))
                return;

            var before = new BeforeCollision2DEnterEvent(other);
            RaiseEvent(gameObject, ref before);
            if (before.Cancelled)
                return;

            RaiseEvent(gameObject, new Collision2DEnterEvent(other));
            _collisionEnter?.Invoke();
            RaiseEvent(gameObject, new AfterCollision2DEnterEvent(other));
        }

        protected void CollisionStay(Collider other)
        {
            RaiseEvent(gameObject, new CollisionStayEvent(other));
            _collisionStay?.Invoke();
        }

        protected void CollisionStay2D(Collider2D other)
        {
            RaiseEvent(gameObject, new Collision2DStayEvent(other));
            _collisionStay?.Invoke();
        }

        protected void CollisionExit(Collider other)
        {
            if (CheckCollisionLink(false))
                return;

            var before = new BeforeCollisionExitEvent(other);
            RaiseEvent(gameObject, ref before);
            if (before.Cancelled)
                return;

            RaiseEvent(gameObject, new CollisionExitEvent(other));
            _collisionExit?.Invoke();
            RaiseEvent(gameObject, new AfterCollisionExitEvent(other));
        }

        protected void CollisionExit2D(Collider2D other)
        {
            if (CheckCollisionLink(false))
                return;

            var before = new BeforeCollision2DExitEvent(other);
            RaiseEvent(gameObject, ref before);
            if (before.Cancelled)
                return;

            RaiseEvent(gameObject, new Collision2DExitEvent(other));
            _collisionExit?.Invoke();
            RaiseEvent(gameObject, new AfterCollision2DExitEvent(other));
        }

        private void OnRelayedCollisionEnterEvent<T>(GameObject i, ref T args)
        {
            _collisionEnter?.Invoke();
        }

        private void OnRelayedCollisionStayEvent<T>(GameObject i, ref T args)
        {
            _collisionStay?.Invoke();
        }

        private void OnRelayedCollisionExitEvent<T>(GameObject i, ref T args)
        {
            _collisionExit?.Invoke();
        }
    }
}