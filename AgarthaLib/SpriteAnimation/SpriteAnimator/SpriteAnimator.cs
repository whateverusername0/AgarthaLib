using UnityEngine;

namespace AgarthaLib.SpriteAnimation.SpriteAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class SpriteAnimator : SpriteAnimatorBase
    {
        public SpriteRenderer SR;

        private void Start()
        {
            SR = SR != null ? SR : GetComponent<SpriteRenderer>();
        }

        protected override void SetFrame(Sprite frame)
            => SR.sprite = frame;
    }
}
