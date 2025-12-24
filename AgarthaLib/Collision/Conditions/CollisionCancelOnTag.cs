using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using System.Linq;

namespace AgarthaLib.Collision.Conditions
{
    public class CollisionCancelOnTag : AgarthanBehaviour
    {
        public CollisionEventTrigger Collider;
        public List<string> Whitelist;
        public List<string> Blacklist;

        private void Start()
        {
            SubscribeEvent<BeforeCollisionEnterEvent>(OnBeforeCollisionEnter);
        }

        private void OnBeforeCollisionEnter(object invoker, ref BeforeCollisionEnterEvent args)
        {
            var target = args.Target;
            var allowed = !Whitelist.Any(q => target.CompareTag(q));
            var disallowed = Blacklist.Any(q => target.CompareTag(q));
            args.Cancelled = allowed || disallowed;
        }
    }
}