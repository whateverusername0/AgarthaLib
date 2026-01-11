using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Sprites.Animation.SpriteAnimator
{
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