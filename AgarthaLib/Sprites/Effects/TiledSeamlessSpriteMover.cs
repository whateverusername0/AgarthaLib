using AgarthaLib.Attributes;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Sprites.Effects
{
    public class TiledSeamlessSpriteMover : AgarthanBehaviour
    {
        [ValidateNull] public SpriteRenderer Renderer;
        public Vector2 Speed = Vector2.down;

        protected override void Update()
        {
            base.Update();

            if (Speed == Vector2.zero
            || Renderer == null
            || Renderer.sprite == null)
                return;

            var sprite = Renderer.sprite;
            var rect = new Vector2(sprite.rect.width, sprite.rect.height);
            var pixels = sprite.pixelsPerUnit;

            var size = rect / pixels;
            var delta = transform.localPosition + (Vector3.one.Multiply((Vector3)Speed) * Time.deltaTime);
            delta = new(delta.x.Loop(size.x), delta.y.Loop(size.y), transform.localPosition.z);

            transform.localPosition = delta;
        }
    }
}