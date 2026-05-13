using AgarthaLib.Attributes;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Sprites.Animation.SpriteAnimator
{
    [RequireComponent(typeof(RawImage))]
    [Obsolete("Please use the Animation.Sprite namespace instead")]
    public class RawImageSpriteAnimator : SpriteAnimatorBase
    {
        [ValidateNull] public RawImage Image;

        protected override void SetFrame(Sprite frame)
            => Image.texture = frame.texture;
    }
}