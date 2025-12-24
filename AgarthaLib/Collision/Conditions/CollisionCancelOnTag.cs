using Agartha.EventSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Agartha.Collision.Conditions
{
    public class CollisionCancelOnTag : MonoBehaviour
    {
        public CollisionEventTrigger Collider;
        public List<string> Whitelist;
        public List<string> Blacklist;

        private void Start()
        {
            Collider.BeforeCollisionEnterEvent += OnBeforeCollision;
        }

        private CancellableEvent OnBeforeCollision(CollisionEventTrigger invoker, GameObject target)
        {
            var allowed = !Whitelist.Any(q => target.CompareTag(q));
            var disallowed = Blacklist.Any(q => target.CompareTag(q));
            return allowed || disallowed;
        }
    }
}