using AgarthaLib.Sprites.Effects;
using UnityEngine;

namespace AgarthaLib._2D.Sprites.Transforms
{
    public class SeamlessSpriteScroller : SeamlessTransformScroller
    {
        public SpriteRenderer Renderer;

        public override Vector2? GetRect()
        {
            if (Renderer == null || Renderer.sprite == null)
                return null;

            var sprite = Renderer.sprite;
            var rect = new Vector2(sprite.rect.width, sprite.rect.height);
            var pixels = sprite.pixelsPerUnit;
            return rect / pixels;
        }
    }
}
