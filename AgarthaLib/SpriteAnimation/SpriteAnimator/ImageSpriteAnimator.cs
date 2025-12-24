using UnityEngine;
using UnityEngine.UI;

namespace Agartha.SpriteAnimation.SpriteAnimator
{
    // copies sprite value to the image.
    // generally not recommended if you have a lot of these.
    [RequireComponent(typeof(Image))]
    public class ImageSpriteAnimator : SpriteAnimatorBase
    {
        public Image Image;

        private void Start()
        {
            Image = Image == null ? GetComponent<Image>() : Image;
        }

        protected override void SetFrame(Sprite frame)
            => Image.sprite = frame;
    }
}