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
    /// <typeparam name="TAnim">A concrete frame animation child class.</typeparam>
    /// <typeparam name="TContainer">A concrete frame animation container child class.</typeparam>
    public abstract class EventfulFrameAnimator<TAnim, TContainer, TFrame> : FrameAnimatorBase<TAnim, TFrame>
        where TAnim : FrameAnimation<TFrame>
        where TContainer : EventfulFrameAnimationContainer<TAnim, TFrame>
        where TFrame : Object
    {
        [SerializeField] protected List<TContainer> _queue = new();
        [SerializeField] protected TContainer CurrentAnimation;

        protected override List<TAnim> GetQueue()
            => _queue.Select(q => q.Animation).ToList();

        protected override TAnim GetCurrentAnimation()
            => CurrentAnimation != null ? CurrentAnimation.Animation : null;
        protected override void SetCurrentAnimation(TAnim value) { }
        protected override void Enqueue(TAnim anim) { }
        protected override void Enqueue(List<TAnim> anims) { }

        protected override void HandleFrame(int frame)
        {
            if (CurrentAnimation.FrameEvents.ContainsKey(frame))
                CurrentAnimation.FrameEvents[frame]?.Invoke();
        }

        public TContainer GetCurrent() => CurrentAnimation;
        public virtual void SetCurrent(TContainer value)
        {
            ResetTime();
            CurrentAnimation = value;
        }

        public override void ClearPlayingAnimation()
        {
            SetCurrent(null);
        }

        public override void ResetQueue()
        {
            _queue.Clear();
            SetCurrent(null);
        }

        public void Enqueue(TContainer anim)
            => _queue.Add(anim);

        public void Enqueue(List<TContainer> anims)
            => _queue.AddRange(anims);
    }
}
