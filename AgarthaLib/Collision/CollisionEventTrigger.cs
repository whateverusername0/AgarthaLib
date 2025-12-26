using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Collision
{
    public class CollisionEventTrigger : AgarthanBehaviour
    {
        private void OnCollisionEnter(UnityEngine.Collision other)
            => BeginCollide(other.collider);

        private void OnCollisionEnter2D(Collision2D other)
            => BeginCollide2D(other.collider);

        private void OnTriggerEnter(Collider other)
            => BeginCollide(other);

        private void OnTriggerEnter2D(Collider2D other)
            => BeginCollide2D(other);

        private void OnCollisionStay(UnityEngine.Collision other)
            => CollisionStay(other.collider);

        private void OnCollisionStay2D(Collision2D other)
            => CollisionStay2D(other.collider);

        private void OnTriggerStay(UnityEngine.Collider other)
            => CollisionStay(other);

        private void OnTriggerStay2D(Collider2D other)
            => CollisionStay2D(other);

        private void OnCollisionExit(UnityEngine.Collision other)
            => CollisionExit(other.collider);

        private void OnCollisionExit2D(Collision2D other)
            => CollisionExit2D(other.collider);

        private void OnTriggerExit(UnityEngine.Collider other)
            => CollisionExit(other);

        private void OnTriggerExit2D(Collider2D other)
            => CollisionExit2D(other);


        /// <summary>
        ///     Raises an <see cref="BeforeCollisionEnterEvent"/> on itself before colliding.
        ///     Then raises <see cref="CollisionEnterEvent"> and <see cref="AfterCollisionEnterEvent">.
        /// </summary>
        /// <param name="other">The other collision.</param>
        protected void BeginCollide(Collider other)
        {
            var before = new BeforeCollisionEnterEvent(other);
            RaiseEvent(gameObject, ref before);
            if (before.Cancelled)
                return;

            RaiseEvent(gameObject, new CollisionEnterEvent(other));
            RaiseEvent(gameObject, new AfterCollisionEnterEvent(other));
        }

        /// <summary>
        ///    Raises an <see cref="BeforeCollision2DEnterEvent"/> on itself before colliding.
        ///    Then raises <see cref="Collision2DEnterEvent"> and <see cref="AfterCollision2DEnterEvent">.
        /// </summary>
        /// <param name="other"></param>
        protected void BeginCollide2D(Collider2D other)
        {
            var before = new BeforeCollision2DEnterEvent(other);
            RaiseEvent(gameObject, ref before);
            if (before.Cancelled)
                return;

            RaiseEvent(gameObject, new Collision2DEnterEvent(other));
            RaiseEvent(gameObject, new AfterCollision2DEnterEvent(other));
        }

        protected void CollisionStay(Collider other)
        {
            RaiseEvent(gameObject, new CollisionStayEvent(other));
        }

        protected void CollisionStay2D(Collider2D other)
        {
            RaiseEvent(gameObject, new Collision2DStayEvent(other));
        }

        protected void CollisionExit(Collider other)
        {
            RaiseEvent(gameObject, new CollisionExitEvent(other));
        }

        protected void CollisionExit2D(Collider2D other)
        {
            RaiseEvent(gameObject, new Collision2DExitEvent(other));
        }
    }
}