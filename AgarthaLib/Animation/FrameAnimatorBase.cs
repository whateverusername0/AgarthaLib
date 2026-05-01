using AgarthaLib.MonoBehavior;
using AgarthaLib.Timing;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Animation
{
    public abstract class FrameAnimatorBase<T> : AgarthanBehaviour where T : FrameAnimation<T>
    {
        protected abstract List<T> GetQueue();
        protected abstract T GetCurrentAnimation();
        protected abstract void SetCurrentAnimation(T value);

        [SerializeField] protected TimeType TimeScale;

        [SerializeField] private double _animTime = 0f;
        [SerializeField] private int _currentFrame = 0;

        protected override void Update()
        {
            base.Update();

            if (TimeScale != TimeType.Normal && TimeScale != TimeType.Unscaled)
                return;

            _animTime += TimeScale == TimeType.Normal ? Time.deltaTime : Time.unscaledDeltaTime;

            Cycle();
        }

        private void LateUpdate()
        {
            if (TimeScale != TimeType.Late && TimeScale != TimeType.LateUnscaled)
                return;

            _animTime += TimeScale == TimeType.LateUnscaled ? Time.unscaledDeltaTime : Time.deltaTime;

            Cycle();
        }

        private void FixedUpdate()
        {
            if (TimeScale != TimeType.Fixed && TimeScale != TimeType.FixedUnscaled)
                return;

            _animTime += TimeScale == TimeType.FixedUnscaled ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;

            Cycle();
        }

        private void Cycle()
        {
            var queue = GetQueue();
            var currentAnimation = GetCurrentAnimation();

            if (currentAnimation == null && (queue == null || queue.Count == 0))
                return;

            if (currentAnimation == null)
                SetCurrentAnimation(queue[0]);

            Cycle(currentAnimation);
        }

        private void Cycle(T anim)
        {
            if (anim == null) return;

            var frame = anim.Frames[_currentFrame];
            SetFrame(frame);
            HandleFrame(_currentFrame);

            if (_animTime >= 1f / anim.FPS)
            {
                _animTime = 0f;
                _currentFrame += 1;

                if (_currentFrame >= anim.Frames.Count)
                {
                    _currentFrame = 0;
                    if (!anim.Loop || GetQueue().Count > 1)
                        MoveNext();
                }
            }
        }

        protected abstract void SetFrame(T frame);
        protected virtual void HandleFrame(int frame) { } // do nothing

        public void ClearPlayingAnimation()
        {
            SetCurrentAnimation(null);
            _currentFrame = 0;
            _animTime = 0f;
        }

        public FrameAnimatorBase<T> MoveNext()
        {
            var queue = GetQueue();

            ClearPlayingAnimation();
            if (queue.Count >= 1)
                queue.RemoveAt(0);
            return this;
        }

        public FrameAnimatorBase<T> Enqueue(T anim)
        {
            GetQueue().Add(anim);
            return this;
        }

        public FrameAnimatorBase<T> Enqueue(List<T> anims)
        {
            GetQueue().AddRange(anims);
            return this;
        }

        public FrameAnimatorBase<T> ResetQueue()
        {
            ClearPlayingAnimation();
            GetQueue().Clear();
            SetCurrentAnimation(null);
            return this;
        }

        public T PlayForce(T anim)
        {
            ResetQueue();
            Enqueue(anim);
            return anim;
        }

        public void PlayForce(List<T> anims)
        {
            ResetQueue();
            Enqueue(anims);
        }
    }
}
