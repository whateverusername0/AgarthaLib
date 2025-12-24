using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.SpriteAnimation.SpriteAnimator
{
    [Serializable] public enum FrameCycleScale
    {
        Normal = 0,
        Unscaled = 1,
        Fixed = 2,
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

    public abstract class SpriteAnimatorBase : MonoBehaviour
    {
        public List<SpriteAnimatorQueueItem> Queue;
        public SpriteAnimatorQueueItem CurrentAnimation;
        public FrameCycleScale TimeScale;
        public bool UseLateUpdate = false;

        [SerializeField] private double _animTime = 0f;
        [SerializeField] private int _currentFrame = 0;

        private void Update()
        {
            if (UseLateUpdate
            || TimeScale != FrameCycleScale.Normal && TimeScale != FrameCycleScale.Unscaled)
                return;

            _animTime += TimeScale == FrameCycleScale.Normal ? Time.deltaTime : Time.unscaledDeltaTime;

            Cycle();
        }

        private void LateUpdate()
        {
            if (!UseLateUpdate
            || TimeScale != FrameCycleScale.Normal && TimeScale != FrameCycleScale.Unscaled)
                return;

            _animTime += TimeScale == FrameCycleScale.Normal ? Time.deltaTime : Time.unscaledDeltaTime;

            Cycle();
        }

        private void FixedUpdate()
        {
            if (TimeScale != FrameCycleScale.Fixed)
                return;

            _animTime = Time.fixedDeltaTime;
            Cycle();
        }

        private void Cycle()
        {
            if (Queue == null || Queue.Count == 0)
                return;

            if (CurrentAnimation == null || CurrentAnimation.Animation == null)
                CurrentAnimation = Queue[0];

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

                    if (!anim.Loop || Queue.Count > 1)
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
            if (Queue.Count >= 1)
                Queue.RemoveAt(0);
            return this;
        }

        public SpriteAnimatorBase Enqueue(SpriteAnimation anim)
        {
            Queue.Add(new(anim));
            return this;
        }

        public SpriteAnimatorBase Enqueue(List<SpriteAnimation> anims)
        {
            Queue.AddRange(anims.Select(q => new SpriteAnimatorQueueItem(q)).ToList());
            return this;
        }

        public SpriteAnimatorBase ResetQueue()
        {
            ClearPlayingAnimation();
            Queue.Clear();
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
            Queue.Last().EndAction = @void;
        }
    }
}