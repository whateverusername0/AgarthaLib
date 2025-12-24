using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.SpriteAnimation.SpriteAnimator
{
    // copies sprite value to the image.
    // generally not recommended if you have a lot of these.
    [RequireComponent(typeof(RawImage))]
    public class RawImageSpriteAnimator : SpriteAnimatorBase
    {
        public RawImage Image;

        private void Start()
        {
            Image = Image == null ? GetComponent<RawImage>() : Image;
        }

        protected override void SetFrame(Sprite frame)
            => Image.texture = frame.texture;
    }
}