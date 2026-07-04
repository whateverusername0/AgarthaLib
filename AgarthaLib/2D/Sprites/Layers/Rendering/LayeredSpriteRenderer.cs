using UnityEngine;

namespace AgarthaLib._2D.Sprites.Layers.Rendering
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LayeredSpriteRenderer : LayeredSpriteRendererBasic<SpriteRenderer>
    {
        protected override void SetMaterial(SpriteRenderer renderer, Material m)
            => renderer.sharedMaterial = m;

        protected override void SetSprite(SpriteRenderer renderer, Sprite s)
            => renderer.sprite = s;
    }
}