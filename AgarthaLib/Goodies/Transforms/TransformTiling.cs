using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Goodies.Transforms
{
    public class TransformTiling : AgarthanBehaviour
    {
        public Vector3 Origin = Vector3.zero;
        public Vector2 Rect = Vector2.zero;

        public virtual Vector2? GetRect()
            => Rect;

        [ContextMenu("Get Rect")]
        public virtual void SetRect()
        {
            var rect = GetRect();
            if (rect != null) Rect = rect.Value;
        }

        protected override void Update()
        {
            base.Update();

            var rect = GetRect();
            if (rect == null) return;

            var lpos = Origin + transform.localPosition;
            lpos = new(lpos.x.Loop(rect.Value.x), lpos.y.Loop(rect.Value.y), transform.localPosition.z);
            transform.localPosition = lpos;
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.localPosition, (Vector3)Rect);
        }
    }
}
