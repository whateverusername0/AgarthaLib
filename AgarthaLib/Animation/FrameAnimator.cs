using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Animation
{
    /// <summary>
    ///     A raw implementation of <see cref="FrameAnimatorBase{TAnim, TFrame}"/>.
    /// </summary>
    /// <inheritdoc/>
    public abstract class FrameAnimator<TAnim, TFrame> : FrameAnimatorBase<TAnim, TFrame>
        where TAnim : FrameAnimation<TFrame>
        where TFrame : Object
    {
        [SerializeField] protected List<TAnim> _queue = new();
        [SerializeField] protected TAnim CurrentAnimation;

        protected override List<TAnim> GetQueue() => _queue;
        protected override TAnim GetCurrentAnimation() => CurrentAnimation;
        protected override void SetCurrentAnimation(TAnim value) => CurrentAnimation = value;
        protected override void HandleFrame(int frame) { }
        protected override void Enqueue(TAnim anim) { }
        protected override void Enqueue(List<TAnim> anims) { }

        public TAnim GetCurrent() => CurrentAnimation;

        public void SetCurrent(TAnim value)
        {
            ResetTime();
            CurrentAnimation = value;
        }

        public override void ClearPlayingAnimation()
        {
            SetCurrent(null);
        }

        public void Enqueue(FrameAnimation<TFrame> anim)
            => _queue.Add((TAnim)anim);

        public void Enqueue(List<FrameAnimation<TFrame>> anims)
            => _queue.AddRange((IEnumerable<TAnim>)anims);

        public override void ResetQueue()
        {
            _queue.Clear();
            SetCurrent(null);
        }
    }
}
