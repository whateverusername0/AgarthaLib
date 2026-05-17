using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Goodies.Transforms
{
    public class InfiniteObjectScroller : AgarthanBehaviour
    {
        public int Amount = 3;
        public BoxCollider Bounds;

        private List<Transform> _transforms;
        private Transform _primary;

        protected override void Update()
        {
            base.Update();


        }
    }
}
