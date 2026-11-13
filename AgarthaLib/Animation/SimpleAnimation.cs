using AgarthaLib.Attributes;
using AgarthaLib.Extensions;
using System;
using UnityEngine;

namespace AgarthaLib.Animation
{
    /// <summary>
    ///     Class for helping with moving a float from point A to point B.
    ///     Supports built in easing functions with different modes.
    /// </summary>
    /// <remarks>
    ///     No cool graphs though. Check <see cref="MathA"/> attributions for them.
    /// </remarks>
    [Serializable] public class SimpleAnimation
    {
        /// <summary>
        ///     Point A.
        /// </summary>
        public float Start = 0f;

        /// <summary>
        ///     Point B.
        /// </summary>
        public float End = 1f;

        /// <summary>
        ///     Duration of the animation.
        /// </summary>
        public float Duration = 1f;

        /// <summary>
        ///     If the animation should end mid way and go back to it's original point.
        /// </summary>
        public bool Spike = false;

        /// <summary>
        ///     Easing mode for the animation.
        ///     This one's In, meaning the animation will start slowly.
        /// </summary>
        public EasingMode EasingIn;

        /// <summary>
        ///     Easing mode for the animation.
        ///     This one's Out, meaning the animation will end slowly.
        /// </summary>
        public EasingMode EasingOut;

        [SerializeField, EditorReadOnly] protected float _time = 0f;

        /// <summary>
        ///     Whether the animation has finished playing or not.
        /// </summary>
        public bool Finished => _time >= Duration;

        public bool IsPlaying => !Finished;

        /// <summary>
        ///     Current timing point from 0 to 1 (start to end).
        /// </summary>
        public float Time => (_time == 0 ? float.Epsilon : _time) / Duration;

        /// <summary>
        ///     Constructor for <see cref="SimpleAnimation"/>.
        /// </summary>
        /// <param name="start"> <inheritdoc cref="Start" path="/summary/node()"/> </param>
        /// <param name="end"> <inheritdoc cref="End" path="/summary/node()"/> </param>
        /// <param name="duration"> <inheritdoc cref="Duration" path="/summary/node()"/> </param>
        /// <param name="spike"> <inheritdoc cref="Spike" path="/summary/node()"/> </param>
        public SimpleAnimation(float start, float end, float duration, bool spike = false)
        {
            Start = start;
            End = end;
            Duration = duration;
            Spike = spike;

            EasingIn = EasingMode.Linear;
            EasingOut = EasingMode.Linear;
        }

        /// <inheritdoc cref="SimpleAnimation(float, float, float, bool)"/>
        /// <param name="easingIn"> <inheritdoc cref="EasingIn" path="/summary/node()"/> </param>
        /// <param name="easingOut"> <inheritdoc cref="EasingOut" path="/summary/node()"/> </param>
        public SimpleAnimation(float start, float end, float duration, bool spike,
            EasingMode easingIn, EasingMode easingOut)
            : this(start, end, duration, spike)
        {
            EasingIn = easingIn;
            EasingOut = easingOut;
        }

        /// <summary> Does a step and progresses the time of the animation. </summary>
        /// <param name="delta"> Time delta. Preferably use <see cref="Time.deltaTime"/>. </param>
        /// <returns> The Lerp between point A and B with eased time. </returns>
        public float Step(float delta)
        {
            _time = Mathf.Clamp(_time + delta, float.Epsilon, Duration);
            return Mathf.Lerp(Start, End, GetTime(_time / Duration));
        }

        /// <returns> The time elapsed with easing functions provided. </returns>
        public float GetTime(float t)
        {
            if (Spike) t = t <= .5f ? t / .5f : (1 - t) / .5f;

            bool hasIn = EasingIn != EasingMode.Linear;
            bool hasOut = EasingOut != EasingMode.Linear;

            return (hasIn, hasOut) switch
            {
                (true, false) => EaseIn(t, EasingIn),
                (false, true) => EaseOut(t, EasingOut),
                (true, true) => Mathf.Lerp(EaseIn(t, EasingIn), EaseOut(t, EasingOut), t),
                _ => t
            };
        }

        protected float EaseIn(float t, EasingMode mode) => mode switch
        {
            EasingMode.Quadratic => MathA.EaseInQuadratic(t),
            EasingMode.Cubic => MathA.EaseInCubic(t),
            EasingMode.Exponential => MathA.EaseInExponential(t),
            _ => t,
        };

        protected float EaseOut(float t, EasingMode mode) => mode switch
        {
            EasingMode.Quadratic => MathA.EaseOutQuadratic(t),
            EasingMode.Cubic => MathA.EaseOutCubic(t),
            EasingMode.Exponential => MathA.EaseOutExponential(t),
            _ => t,
        };
    }
}
