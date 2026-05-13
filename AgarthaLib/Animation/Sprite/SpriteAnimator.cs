using UnityEngine;

namespace AgarthaLib.Animation.Sprite
{
    public class SpriteAnimator : FrameAnimator<SpriteAnimation, UnityEngine.Sprite>
    {
        public SpriteRenderer Renderer;

        protected override void SetFrame(UnityEngine.Sprite frame)
            => Renderer.sprite = frame;
    }
}
