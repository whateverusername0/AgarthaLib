using UnityEngine;

namespace AgarthaLib.Animation.Sprite
{
    public class EventfulSpriteAnimator
        : EventfulFrameAnimator<SpriteAnimation, EventfulSpriteAnimationContainer, UnityEngine.Sprite>
    {
        public SpriteRenderer Renderer;

        protected override void SetFrame(UnityEngine.Sprite frame)
            => Renderer.sprite = frame;
    }
}
