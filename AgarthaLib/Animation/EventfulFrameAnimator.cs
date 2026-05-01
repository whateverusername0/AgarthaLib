using AgarthaLib.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Animation
{
    /// <summary>
    ///     An implementation of <see cref="FrameAnimatorBase{T}"/> that supports per-frame events.
    ///     All added animations are stored in components of children transforms of the animator.
    ///     And events are invoked per each frame played.
    /// </summary>
    /// <typeparam name="T">A concrete frame animation child class.</typeparam>
    /// <typeparam name="C">A concrete frame animation container child class.</typeparam>
    public abstract class EventfulFrameAnimator<T, C> : FrameAnimatorBase<T>
    where T : FrameAnimation<T>
    where C : EventfulFrameAnimationContainer<T>
    {
        [SerializeField] protected List<EventfulFrameAnimationContainer<T>> _queue = new();
        [SerializeField] protected EventfulFrameAnimationContainer<T> CurrentAnimation;

        protected override List<T> GetQueue()
            => _queue.Select(q => q.Animation).ToList();

        protected override T GetCurrentAnimation()
            => CurrentAnimation.Animation;

        protected override void SetCurrentAnimation(T value)
        {
            var ct = this.transform.EnsureChild(value.name);
            var efa = ct.EnsureComponent<C>();
            efa.Animation = value;
        }

        protected override void HandleFrame(int frame)
        {
            base.HandleFrame(frame);

            // invoke said event
            if (CurrentAnimation.FrameEvents.ContainsKey(frame))
                CurrentAnimation.FrameEvents[frame]?.Invoke();
        }
    }
}
