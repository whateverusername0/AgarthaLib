using AgarthaLib.Data;

namespace AgarthaLib.Collision.Conditions
{
    public class CollisionQueryCondition : CollisionCondition
    {
        public GameObjectQuery Query = new();

        protected override void OnBeforeCollisionEnter<T>(object invoker, ref T args)
            => args.Cancelled = args.Cancelled || !Query.Matches(args.GameObject);
    }
}
