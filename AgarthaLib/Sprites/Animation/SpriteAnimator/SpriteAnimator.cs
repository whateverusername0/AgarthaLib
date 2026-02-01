using AgarthaLib.Attributes;
using UnityEngine;

namespace AgarthaLib.Sprites.Animation.SpriteAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class SpriteAnimator : SpriteAnimatorBase
    {
        [ValidateNull] public SpriteRenderer SR;

        protected override void SetFrame(Sprite frame)
            => SR.sprite = frame;
    }
}
