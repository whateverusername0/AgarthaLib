using AgarthaLib.EventSystem;
using UnityEngine;

namespace AgarthaLib.Collision
{
    public abstract class CollisionEventBase : EventBase
    {
        public Collider Other;
        public GameObject GameObject;

        public CollisionEventBase(Collider other)
        {
            Other = other;
            GameObject = other.gameObject;
        }
    }

    public abstract class Collision2DEventBase : EventBase
    {
        public Collider2D Other;
        public GameObject GameObject;

        public Collision2DEventBase(Collider2D other)
        {
            Other = other;
            GameObject = other.gameObject;
        }
    }

    public abstract class CancellableCollisionEventBase : CollisionEventBase
    {
        public bool Cancelled { get; set; }
        public CancellableCollisionEventBase(Collider other) : base(other) { }
    }

    public abstract class CancellableCollision2DEventBase : Collision2DEventBase
    {
        public bool Cancelled { get; set; }
        public CancellableCollision2DEventBase(Collider2D other) : base(other) { }
    }

    #region Events

    public class BeforeCollisionEnterEvent : CancellableCollisionEventBase
    {
        public BeforeCollisionEnterEvent(Collider other) : base(other) { }
    }

    public class BeforeCollision2DEnterEvent : CancellableCollision2DEventBase
    {
        public BeforeCollision2DEnterEvent(Collider2D other) : base(other) { }
    }

    public class CollisionEnterEvent : CollisionEventBase
    {
        public CollisionEnterEvent(Collider other) : base(other) { }
    }

    public class Collision2DEnterEvent : Collision2DEventBase
    {
        public Collision2DEnterEvent(Collider2D other) : base(other) { }
    }

    public class AfterCollisionEnterEvent : CollisionEventBase
    {
        public AfterCollisionEnterEvent(Collider other) : base(other) { }
    }

    public class AfterCollision2DEnterEvent : Collision2DEventBase
    {
        public AfterCollision2DEnterEvent(Collider2D other) : base(other) { }
    }

    public class CollisionStayEvent : CollisionEventBase
    {
        public CollisionStayEvent(Collider other) : base(other) { }
    }

    public class Collision2DStayEvent : Collision2DEventBase
    {
        public Collision2DStayEvent(Collider2D other) : base(other) { }
    }

    public class CollisionExitEvent : CollisionEventBase
    {
        public CollisionExitEvent(Collider other) : base(other) { }
    }

    public class Collision2DExitEvent : Collision2DEventBase
    {
        public Collision2DExitEvent(Collider2D other) : base(other) { }
    }

    #endregion
}