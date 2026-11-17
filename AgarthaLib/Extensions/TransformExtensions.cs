using AgarthaLib.Animation;
using AgarthaLib.Data;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections;
using UnityEngine;

namespace AgarthaLib.Extensions
{
    [Serializable] public enum TransformType
    {
        Delta, Absolute
    }

    [Serializable] public enum TransformFlags
    {
        Position, Rotation, Scale, All
    }

    public static class TransformExtensions
    {
        public static Transform OverrideTransformData(this Transform t, TransformData @override)
        {
            t.localPosition = @override.Position;
            t.localEulerAngles = @override.EulerAngles;
            t.localScale = @override.Scale;
            return t;
        }

        private static CoroutineManager _coroutine => CoroutineManager.Instance;

        public static void Animate(this Transform t, TransformData @override, float duration,
            EasingMode easeIn, EasingMode easeOut, bool spike = false)
        {
            var sa = new SimpleAnimation(0, 1, duration, spike, easeIn, easeOut);
            _coroutine.Add(t.IEAnimate(new(t), @override, sa));
        }

        private static IEnumerator IEAnimate(this Transform t, TransformData a, TransformData b, SimpleAnimation sa)
        {
            var wof = new WaitForEndOfFrame();
            while (sa.IsPlaying)
            {
                var td = TransformData.Lerp(a, b, sa.Step(Time.deltaTime));
                t.OverrideTransformData(td);
                yield return wof;
            }
        }
    }
}
