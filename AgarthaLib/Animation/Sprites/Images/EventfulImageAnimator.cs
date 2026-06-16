using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Animation.Sprites.Images
{
    public class EventfulRawImageAnimator
        : EventfulFrameAnimator<SpriteAnimation, EventfulSpriteAnimationContainer, Sprite>
    {
        public Image Renderer;

        protected override void SetFrame(Sprite frame)
            => Renderer.sprite = frame;
    }
}
