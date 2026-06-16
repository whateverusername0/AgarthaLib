using UnityEngine;

namespace AgarthaLib.Animation.Sprites
{
    public class EventfulSpriteAnimator
        : EventfulFrameAnimator<SpriteAnimation, EventfulSpriteAnimationContainer, Sprite>
    {
        public SpriteRenderer Renderer;

        protected override void SetFrame(Sprite frame)
            => Renderer.sprite = frame;
    }
}
