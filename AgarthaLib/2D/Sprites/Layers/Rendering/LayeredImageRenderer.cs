using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib._2D.Sprites.Layers.Rendering
{
    [RequireComponent(typeof(Image))]
    public class LayeredImageRenderer : LayeredSpriteRendererBase<Image>
    {
        protected override int GetOrderInLayer(Image renderer) => 0;

        protected override void SetOrderInLayer(Image renderer, int order) { }

        protected override void SetMaterial(Image renderer, Material mat)
            => renderer.material = mat;

        protected override void SetSprite(Image renderer, Sprite sprite)
            => renderer.sprite = sprite;
    }
}
