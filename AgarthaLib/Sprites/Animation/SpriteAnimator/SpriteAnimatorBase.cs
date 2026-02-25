using AgarthaLib.MonoBehavior;
using AgarthaLib.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Sprites.Animation.SpriteAnimator
{
    public abstract class SpriteAnimatorBase : AgarthanBehaviour
    {
        [SerializeField] protected List<SpriteAnimatorQueueItem> _queue;
        [SerializeField] protected SpriteAnimatorQueueItem CurrentAnimation;
        [SerializeField] protected TimeType TimeScale;
        [SerializeField] protected bool UseLateUpdate = false;

        [SerializeField] private double _animTime = 0f;
        [SerializeField] private int _currentFrame = 0;

        protected override void Update()
        {
            base.Update();

            if (UseLateUpdate || TimeScale != TimeType.Normal && TimeScale != TimeType.Unscaled)
                return;

            _animTime += TimeScale == TimeType.Normal ? Time.deltaTime : Time.unscaledDeltaTime;

            Cycle();
        }

        private void LateUpdate()
        {
            if (!UseLateUpdate || TimeScale != TimeType.Normal && TimeScale != TimeType.Unscaled)
                return;

            _animTime += TimeScale == TimeType.Normal ? Time.deltaTime : Time.unscaledDeltaTime;

            Cycle();
        }

        private void FixedUpdate()
        {
            if (TimeScale != TimeType.Fixed)
                return;

            _animTime += Time.fixedDeltaTime;

            Cycle();
        }

        private void Cycle()
        {
            if (_queue == null || _queue.Count == 0)
                return;

            if (CurrentAnimation == null || CurrentAnimation.Animation == null)
                CurrentAnimation = _queue[0];

            Cycle(CurrentAnimation);
        }

        private void Cycle(SpriteAnimatorQueueItem item)
        {
            var anim = item.Animation;
            if (!anim) return;

            SetFrame(anim.Frames[_currentFrame]);

            if (_animTime >= 1f / anim.FPS)
            {
                _animTime = 0f;
                _currentFrame += 1;

                if (_currentFrame >= anim.Frames.Count)
                {
                    _currentFrame = 0;
                    item.EndAction?.Invoke();

                    if (!anim.Loop || _queue.Count > 1)
                        MoveNext();
                }
            }
        }

        protected abstract void SetFrame(Sprite frame);

        public void ClearPlayingAnimation()
        {
            CurrentAnimation = null;
            _currentFrame = 0;
            _animTime = 0f;
        }

        public SpriteAnimatorBase MoveNext()
        {
            ClearPlayingAnimation();
            if (_queue.Count >= 1)
                _queue.RemoveAt(0);
            return this;
        }

        public SpriteAnimatorBase Enqueue(SpriteAnimation anim)
        {
            _queue.Add(new(anim));
            return this;
        }

        public SpriteAnimatorBase Enqueue(List<SpriteAnimation> anims)
        {
            _queue.AddRange(anims.Select(q => new SpriteAnimatorQueueItem(q)).ToList());
            return this;
        }

        public SpriteAnimatorBase ResetQueue()
        {
            ClearPlayingAnimation();
            _queue.Clear();
            CurrentAnimation = null;
            return this;
        }

        public void PlayForce(SpriteAnimationBase anim)
        {
            if (anim is not SpriteAnimation && anim is not SpriteAnimationComposite)
                throw new ArgumentException($"{anim.GetType()} is not a valid sprite animation or it's derivative!");

            switch (anim)
            {
                case SpriteAnimation @base: PlayForce(@base); break;
                case SpriteAnimationComposite comp: PlayForce(comp.Animations); break;
            }
        }

        public SpriteAnimation PlayForce(SpriteAnimation anim)
        {
            ResetQueue();
            Enqueue(anim);
            return anim;
        }

        public void PlayForce(List<SpriteAnimation> anims)
        {
            ResetQueue();
            Enqueue(anims);
        }

        public void SetLastFrameAction(Action @void)
        {
            _queue.Last().EndAction = @void;
        }
    }

    [Serializable] public class SpriteAnimatorQueueItem
    {
        public SpriteAnimation Animation;
        public Action EndAction;

        public SpriteAnimatorQueueItem(SpriteAnimation anim)
        {
            Animation = anim;
        }
    }
}