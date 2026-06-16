using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Animation.Sprites.RawImages
{
    public class EventfulRawImageAnimator
        : EventfulFrameAnimator<SpriteAnimation, EventfulSpriteAnimationContainer, Sprite>
    {
        public RawImage Renderer;

        protected override void SetFrame(Sprite frame)
            => Renderer.texture = frame.texture;
    }
}
