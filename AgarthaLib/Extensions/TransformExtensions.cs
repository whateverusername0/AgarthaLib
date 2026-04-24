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
        private static CoroutineManager _coroutine => CoroutineManager.Instance;

        public static CoroutineData SmoothMoveCoroutine(this Transform t, Vector3 delta,
            float duration, bool ease, TransformType type)
            => _coroutine.Add(IEMove(t, delta, duration, ease, type));

        private static IEnumerator IEMove(Transform tr, Vector3 delta,
            float duration, bool ease, TransformType type)
        {
            if (tr == null) yield break;

            var start = tr.localPosition;
            var end = type == TransformType.Delta ? start + delta : delta;

            if (duration <= 0f)
            {
                tr.localPosition = end;
                yield break;
            }

            var elapsed = 0f;
            var wfe = new WaitForEndOfFrame();
            while (elapsed < duration)
            {
                yield return wfe;
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                var p = ease ? Mathf.SmoothStep(0f, 1f, t) : t;
                tr.localPosition = Vector3.Lerp(start, end, p);
            }

            // ensure exact final position
            tr.localPosition = end;
            yield break;
        }

        public static CoroutineData SmoothRotateCoroutine(this Transform t, Quaternion delta,
            float duration, bool ease, TransformType type)
            => _coroutine.Add(IERotate(t, delta, duration, ease, type));

        private static IEnumerator IERotate(Transform tr, Quaternion delta,
            float duration, bool ease, TransformType type)
        {
            if (tr == null) yield break;

            var start = tr.localRotation;
            var end = type == TransformType.Delta ? start * delta : delta;

            if (duration <= 0f)
            {
                tr.localRotation = end;
                yield break;
            }

            var elapsed = 0f;
            var wfe = new WaitForEndOfFrame();
            while (elapsed < duration)
            {
                yield return wfe;
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                var p = ease ? Mathf.SmoothStep(0f, 1f, t) : t;
                tr.localRotation = Quaternion.Slerp(start, end, p);
            }

            // ensure exact final rotation
            tr.localRotation = end;
            yield break;
        }
    }
}
