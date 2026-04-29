using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Sprites.Layers.Rendering
{
    [RequireComponent(typeof(RawImage))]
    public class LayeredRawImageRenderer : LayeredSpriteRendererBase<RawImage>
    {
        protected override int GetOrderInLayer(RawImage renderer) => 0;

        protected override void SetOrderInLayer(RawImage renderer, int order) { }

        protected override void SetMaterial(RawImage renderer, Material mat)
            => renderer.material = mat;

        protected override void SetSprite(RawImage renderer, Sprite sprite)
            => renderer.texture = sprite.texture;
    }
}