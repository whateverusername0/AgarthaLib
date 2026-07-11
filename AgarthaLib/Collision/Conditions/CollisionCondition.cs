using AgarthaLib.MonoBehavior;

namespace AgarthaLib.Collision.Conditions
{
    public abstract class CollisionCondition : AgarthanBehaviour
    {
        protected override void Start()
        {
            base.Start();
            SubscribeEvent<BeforeCollisionEnterEvent>(OnBeforeCollisionEnter);
            SubscribeEvent<BeforeCollision2DEnterEvent>(OnBeforeCollisionEnter);
        }

        protected abstract void OnBeforeCollisionEnter<T>(object invoker, ref T args)
            where T : CancellableCollisionEventBase;
    }
}
