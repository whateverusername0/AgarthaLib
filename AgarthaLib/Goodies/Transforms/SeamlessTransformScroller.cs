using AgarthaLib.Attributes;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Sprites.Effects
{
    public abstract class SeamlessTransformScroller : AgarthanBehaviour
    {
        public Vector2 Speed = Vector2.down;

        protected override void Update()
        {
            base.Update();

            if (Speed == Vector2.zero)
                return;

            var rect = GetRect();
            if (rect == null) return;

            var delta = transform.localPosition + (Vector3.one.Multiply((Vector3)Speed) * Time.deltaTime);
            delta = new(delta.x.Loop(rect.Value.x), delta.y.Loop(rect.Value.y), transform.localPosition.z);

            transform.localPosition = delta;
        }

        public abstract Vector2? GetRect();
    }
}