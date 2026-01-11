using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using System.Linq;

namespace AgarthaLib.Collision.Conditions
{
    public class CollisionTagCondition : AgarthanBehaviour
    {
        public List<string> Whitelist;
        public List<string> Blacklist;

        private void Start()
        {
            SubscribeEvent<BeforeCollisionEnterEvent>(OnBeforeCollisionEnter);
        }

        private void OnBeforeCollisionEnter(object invoker, ref BeforeCollisionEnterEvent args)
        {
            if (args.Cancelled) return;

            var target = args.GameObject;
            var allowed = !Whitelist.Any(q => target.CompareTag(q));
            var disallowed = Blacklist.Any(q => target.CompareTag(q));
            args.Cancelled = allowed || disallowed;
        }
    }
}