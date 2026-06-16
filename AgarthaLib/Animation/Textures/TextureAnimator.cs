using UnityEngine;

namespace AgarthaLib.Animation.Textures
{
    public class TextureAnimator : FrameAnimator<TextureAnimation, Texture>
    {
        public Material Material;

        protected override void SetFrame(Texture frame)
            => Material.mainTexture = frame;
    }
}
