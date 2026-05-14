using AgarthaLib.EventSystem;
using UnityEngine;

namespace AgarthaLib.Collision
{
    #region Base

    public abstract class CollisionEventBase : EventBase
    {
        public GameObject GameObject;

        public CollisionEventBase(GameObject gameObject)
            => GameObject = gameObject;
    }

    public abstract class CancellableCollisionEventBase : CollisionEventBase
    {
        public bool Cancelled { get; set; }

        public CancellableCollisionEventBase(GameObject gameObject)
            : base(gameObject) { }
    }

    public abstract class CollisionEvent : CollisionEventBase
    {
        public Collider Other;

        public CollisionEvent(Collider other) : base(other.gameObject)
            => Other = other;
    }

    public abstract class Collision2DEvent : CollisionEventBase
    {
        public Collider2D Other;

        public Collision2DEvent(Collider2D other) : base(other.gameObject)
        {
            Other = other;
            GameObject = other.gameObject;
        }
    }

    public abstract class CancellableCollisionEvent : CancellableCollisionEventBase
    {
        public Collider Other;

        public CancellableCollisionEvent(Collider other)
            : base(other.gameObject) { }
    }

    public abstract class CancellableCollision2DEvent : CancellableCollisionEventBase
    {
        public Collider2D Other;

        public CancellableCollision2DEvent(Collider2D other)
            : base(other.gameObject) { }
    }

    #endregion

    #region Events

    public class BeforeCollisionEnterEvent : CancellableCollisionEvent
    {
        public BeforeCollisionEnterEvent(Collider other) : base(other) { }
    }

    public class BeforeCollision2DEnterEvent : CancellableCollision2DEvent
    {
        public BeforeCollision2DEnterEvent(Collider2D other) : base(other) { }
    }

    public class CollisionEnterEvent : CollisionEvent
    {
        public CollisionEnterEvent(Collider other) : base(other) { }
    }

    public class Collision2DEnterEvent : Collision2DEvent
    {
        public Collision2DEnterEvent(Collider2D other) : base(other) { }
    }

    public class AfterCollisionEnterEvent : CollisionEvent
    {
        public AfterCollisionEnterEvent(Collider other) : base(other) { }
    }

    public class AfterCollision2DEnterEvent : Collision2DEvent
    {
        public AfterCollision2DEnterEvent(Collider2D other) : base(other) { }
    }

    public class CollisionStayEvent : CollisionEvent
    {
        public CollisionStayEvent(Collider other) : base(other) { }
    }

    public class Collision2DStayEvent : Collision2DEvent
    {
        public Collision2DStayEvent(Collider2D other) : base(other) { }
    }

    public class BeforeCollisionExitEvent : CancellableCollisionEvent
    {
        public BeforeCollisionExitEvent(Collider other) : base(other) { }
    }

    public class BeforeCollision2DExitEvent : CancellableCollision2DEvent
    {
        public BeforeCollision2DExitEvent(Collider2D other) : base(other) { }
    }

    public class CollisionExitEvent : CollisionEvent
    {
        public CollisionExitEvent(Collider other) : base(other) { }
    }

    public class Collision2DExitEvent : Collision2DEvent
    {
        public Collision2DExitEvent(Collider2D other) : base(other) { }
    }

    public class AfterCollisionExitEvent : CollisionEvent
    {
        public AfterCollisionExitEvent(Collider other) : base(other) { }
    }

    public class AfterCollision2DExitEvent : Collision2DEvent
    {
        public AfterCollision2DExitEvent(Collider2D other) : base(other) { }
    }

    #endregion
}