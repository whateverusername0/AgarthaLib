using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using AgarthaLib.Timing;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Animation
{
    /// <summary>
    ///     An abstract definition of a class that's supposed to set frames based on timing,
    ///     akin to a keyframe animator.
    ///     It really is just an animator.
    /// </summary>
    /// <typeparam name="TAnim"> A concrete frame animation class. </typeparam>
    /// <typeparam name="TFrame"> A concrete frame type. </typeparam>
    public abstract class FrameAnimatorBase<TAnim, TFrame> : AgarthanBehaviour
        where TAnim : FrameAnimation<TFrame>
        where TFrame : Object
    {
        protected abstract List<TAnim> GetQueue();
        protected abstract TAnim GetCurrentAnimation();
        protected abstract void SetCurrentAnimation(TAnim value);

        [SerializeField] protected TimeType TimeScale;

        [SerializeField, EditorReadOnly] protected double _animationTime = 0f;
        [SerializeField, EditorReadOnly] protected int _currentFrame = 0;

        protected override void Update()
        {
            base.Update();

            if (TimeScale != TimeType.Normal && TimeScale != TimeType.Unscaled)
                return;

            _animationTime += TimeScale == TimeType.Normal
                ? Time.deltaTime
                : Time.unscaledDeltaTime;

            Cycle();
        }

        protected virtual void LateUpdate()
        {
            if (TimeScale != TimeType.Late && TimeScale != TimeType.LateUnscaled)
                return;

            _animationTime += TimeScale == TimeType.LateUnscaled
                ? Time.unscaledDeltaTime
                : Time.deltaTime;

            Cycle();
        }

        protected virtual void FixedUpdate()
        {
            if (TimeScale != TimeType.Fixed && TimeScale != TimeType.FixedUnscaled)
                return;

            _animationTime += TimeScale == TimeType.FixedUnscaled
                ? Time.fixedUnscaledDeltaTime
                : Time.fixedDeltaTime;

            Cycle();
        }

        protected virtual void Cycle()
        {
            var queue = GetQueue();
            var currentAnimation = GetCurrentAnimation();

            if (currentAnimation == null && (queue == null || queue.Count == 0))
            {
                _animationTime = 0f;
                return;
            }

            if (currentAnimation == null)
                SetCurrentAnimation(queue[0]);

            Cycle(currentAnimation);
        }

        protected virtual void Cycle(TAnim anim)
        {
            if (anim == null || anim.Frames.Count == 0)
                return;

            var frame = anim.Frames[_currentFrame];
            SetFrame(frame);

            if (_animationTime >= 1f / anim.FPS)
            {
                _animationTime = 0f;
                _currentFrame += 1;
                HandleFrame(_currentFrame);

                if (_currentFrame >= anim.Frames.Count)
                {
                    _currentFrame = 0;
                    if (!anim.Loop || GetQueue().Count > 1)
                        MoveNext();
                }
            }
        }

        public virtual void ResetTime()
        {
            _currentFrame = 0;
            _animationTime = 0f;
        }

        protected abstract void SetFrame(TFrame frame);
        protected abstract void HandleFrame(int frame);

        public abstract void ClearPlayingAnimation();

        public virtual void MoveNext()
        {
            var queue = GetQueue();

            ClearPlayingAnimation();
            if (queue.Count >= 1)
                queue.RemoveAt(0);
        }

        protected abstract void Enqueue(TAnim anim);
        protected abstract void Enqueue(List<TAnim> anims);

        public abstract void ResetQueue();
    }
}
