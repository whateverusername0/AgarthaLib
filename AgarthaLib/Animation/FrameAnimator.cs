using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Animation
{
    /// <summary>
    ///     A raw implementation of <see cref="FrameAnimatorBase{T}"/>.
    /// </summary>
    public abstract class FrameAnimator<T> : FrameAnimatorBase<T> where T : FrameAnimation<T>
    {
        [SerializeField] protected List<T> _queue = new();
        [SerializeField] protected T CurrentAnimation;

        protected override List<T> GetQueue()
            => _queue;

        protected override T GetCurrentAnimation()
            => CurrentAnimation;

        protected override void SetCurrentAnimation(T value)
            => CurrentAnimation = value;
    }
}
