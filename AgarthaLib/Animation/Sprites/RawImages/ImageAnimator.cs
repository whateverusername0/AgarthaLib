using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Animation.Sprites.RawImages
{
    public class RawImageAnimator : FrameAnimator<SpriteAnimation, Sprite>
    {
        public RawImage Renderer;

        protected override void SetFrame(Sprite frame)
            => Renderer.texture = frame.texture;
    }
}