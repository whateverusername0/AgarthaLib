using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Animation.Sprites.Images
{
    public class RawImageAnimator : FrameAnimator<SpriteAnimation, Sprite>
    {
        public Image Renderer;

        protected override void SetFrame(Sprite frame)
            => Renderer.sprite = frame;
    }
}