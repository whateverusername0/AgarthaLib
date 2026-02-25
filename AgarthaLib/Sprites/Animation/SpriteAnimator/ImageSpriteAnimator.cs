using AgarthaLib.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Sprites.Animation.SpriteAnimator
{
    [RequireComponent(typeof(Image))]
    public class ImageSpriteAnimator : SpriteAnimatorBase
    {
        [ValidateNull] public Image Image;

        protected override void SetFrame(Sprite frame)
            => Image.sprite = frame;
    }
}