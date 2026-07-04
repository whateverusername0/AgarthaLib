using UnityEngine;

namespace AgarthaLib._2D.Sprites.Layers
{
    public abstract class LayeredSpriteRendererBasic<T> : LayeredSpriteRendererBase<T> where T : Renderer
    {
        protected override void SetOrderInLayer(T renderer, int order)
            => renderer.sortingOrder = order;

        protected override int GetOrderInLayer(T renderer)
            => renderer.sortingOrder;
    }
}
