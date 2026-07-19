using AgarthaLib.Data;

namespace AgarthaLib.Collision.Conditions
{
    public class CollisionTagCondition : CollisionCondition
    {
        public ObjectWhitelist<string> Whitelist;

        protected override void OnBeforeCollisionEnter<T>(object invoker, ref T args)
            => args.Cancelled = args.Cancelled || !Whitelist.Pass(args.GameObject.tag);
    }
}