using UnityEngine;

namespace AgarthaLib.Animation.Textures
{
    public class EventfulTextureAnimator
        : EventfulFrameAnimator<TextureAnimation, EventfulTextureAnimationContainer, Texture>
    {
        public Material Material;

        protected override void SetFrame(Texture frame)
            => Material.mainTexture = frame;
    }
}
