using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Animation.Sprites.Images
{
    public class EventfulImageAnimator
        : EventfulFrameAnimator<SpriteAnimation, EventfulSpriteAnimationContainer, Sprite>
    {
        public Image Renderer;

        protected override void SetFrame(Sprite frame)
            => Renderer.sprite = frame;
    }
}
