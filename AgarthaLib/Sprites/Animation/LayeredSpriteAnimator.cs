using AgarthaLib.Sprites.Layers.Rendering;
using UnityEngine;

namespace AgarthaLib.Sprites.Animation.SpriteAnimator
{
    public class LayeredSpriteAnimator : SpriteAnimatorBase
    {
        public LayeredSpriteRendererBase Renderer { get; private set; }

        protected override void SetFrame(Sprite frame)
        {
            Renderer.Sprite = frame;
        }
    }
}
