using AgarthaLib.Data;
using AgarthaLib.MonoBehavior;

namespace AgarthaLib.Collision.Conditions
{
    public class CollisionTagCondition : AgarthanBehaviour
    {
        public ObjectWhitelist<string> Whitelist;

        protected override void Start()
        {
            base.Start();
            SubscribeEvent<BeforeCollisionEnterEvent>(OnBeforeCollisionEnter);
            SubscribeEvent<BeforeCollision2DEnterEvent>(OnBeforeCollisionEnter);
        }

        private void OnBeforeCollisionEnter<T>(object invoker, ref T args)
            where T : CancellableCollisionEventBase
        {
            if (args.Cancelled) return;

            var target = args.GameObject;
            args.Cancelled = !Whitelist.Pass(target.tag);
        }
    }
}