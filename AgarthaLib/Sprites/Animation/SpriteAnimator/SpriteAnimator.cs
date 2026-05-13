using AgarthaLib.Attributes;
using System;
using UnityEngine;

namespace AgarthaLib.Sprites.Animation.SpriteAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    [Obsolete("Please use the Animation.Sprite namespace instead")]
    public sealed class SpriteAnimator : SpriteAnimatorBase
    {
        [ValidateNull] public SpriteRenderer SR;

        protected override void SetFrame(Sprite frame)
            => SR.sprite = frame;
    }
}
