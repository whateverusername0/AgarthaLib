using UnityEngine;

namespace AgarthaLib.Animation.Sprites
{
    public class SpriteAnimator : FrameAnimator<SpriteAnimation, Sprite>
    {
        public SpriteRenderer Renderer;

        protected override void SetFrame(Sprite frame)
            => Renderer.sprite = frame;
    }
}
