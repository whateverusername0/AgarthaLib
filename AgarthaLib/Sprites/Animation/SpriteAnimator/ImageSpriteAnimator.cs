using AgarthaLib.Attributes;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Sprites.Animation.SpriteAnimator
{
    [RequireComponent(typeof(Image))]
    [Obsolete("Please use the Animation.Sprite namespace instead")]
    public class ImageSpriteAnimator : SpriteAnimatorBase
    {
        [ValidateNull] public Image Image;

        protected override void SetFrame(Sprite frame)
            => Image.sprite = frame;
    }
}