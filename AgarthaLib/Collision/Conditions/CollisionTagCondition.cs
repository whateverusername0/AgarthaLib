using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Collision.Conditions
{
    public class CollisionTagCondition : AgarthanBehaviour
    {
        public List<string> Whitelist;
        public List<string> Blacklist;

        protected override void Start()
        {
            base.Start();
            SubscribeEvent<BeforeCollisionEnterEvent>(OnBeforeCollisionEnter);
            SubscribeEvent<BeforeCollision2DEnterEvent>(OnBeforeCollision2DEnter);
        }

        private void OnBeforeCollisionEnter(object invoker, ref BeforeCollisionEnterEvent args)
        {
            if (args.Cancelled) return;

            var target = args.GameObject;
            var allowed = Whitelist.IsValid() && !Whitelist.Any(q => target.CompareTag(q));
            var disallowed = Blacklist.IsValid() && Blacklist.Any(q => target.CompareTag(q));
            args.Cancelled = allowed || disallowed;
        }

        private void OnBeforeCollision2DEnter(GameObject invoker, ref BeforeCollision2DEnterEvent args)
        {
            if (args.Cancelled) return;

            var target = args.GameObject;
            var allowed = Whitelist.IsValid() && !Whitelist.Any(q => target.CompareTag(q));
            var disallowed = Blacklist.IsValid() && Blacklist.Any(q => target.CompareTag(q));
            args.Cancelled = allowed || disallowed;
        }
    }
}