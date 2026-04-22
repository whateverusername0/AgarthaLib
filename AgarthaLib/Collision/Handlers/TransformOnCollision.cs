using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections;
using UnityEngine;

namespace AgarthaLib.Collision.Handlers
{
    public class TransformOnCollision : AgarthanBehaviour
    {
        [Serializable] public enum TransformType
        {
            Delta, Absolute
        }

        [Serializable] public enum TransformFlags
        {
            Position, Rotation, All
        }

        [Header("Properties")]
        [SerializeField] private GameObject _target;
        public GameObject Target => _target != null ? _target : this.gameObject;

        [SerializeField] private TransformFlags Flag = TransformFlags.All;
        [SerializeField] private TransformType Type = TransformType.Delta;
        public Vector3 Position = Vector3.zero;
        public Quaternion Rotation = Quaternion.identity;
        
        [Header("Timing")]
        public float Duration = 1f;
        public bool Ease = true;

        [Header("Trigger")]
        public bool RevertTrigger = false;

        [Header("Private")]
        [SerializeField, EditorReadOnly] private Vector3 _lastPosition;
        [SerializeField, EditorReadOnly] private Quaternion _lastRotation;
        [SerializeField, EditorReadOnly] private bool _triggered = false;

        protected override void Start()
        {
            base.Start();

            _lastPosition = Target.transform.localPosition;
            _lastRotation = Target.transform.localRotation;

            SubscribeEvent<CollisionEnterEvent>(OnCollisionEnterEvent);
            SubscribeEvent<Collision2DEnterEvent>(OnCollision2DEnterEvent);
            SubscribeEvent<CollisionExitEvent>(OnCollisionExitEvent);
            SubscribeEvent<Collision2DExitEvent>(OnCollision2DExitEvent);
        }

        private void OnDrawGizmosSelected()
        {
            if (Target == null) return;

            Gizmos.color = Color.red;
            var t = Target.transform;
            Gizmos.DrawRay(t.position, Position);
            if (Rotation.eulerAngles.magnitude > 0)
                Gizmos.DrawRay(t.position + t.forward, Rotation * t.forward);
        }

        private void OnCollisionEnterEvent(GameObject invoker, ref CollisionEnterEvent args)
        {
            if (_triggered) return;
            _triggered = true;

            Transform(Target, Position, Rotation, Duration, Ease, Type);
        }

        private void OnCollision2DEnterEvent(GameObject invoker, ref Collision2DEnterEvent args)
        {
            if (_triggered) return;
            _triggered = true;

            Transform(Target, Position, Rotation, Duration, Ease, Type);
        }

        private void OnCollisionExitEvent(GameObject invoker, ref CollisionExitEvent args)
        {
            if (!_triggered || !RevertTrigger) return;
            _triggered = false;

            Transform(Target, _lastPosition, _lastRotation, Duration, Ease, TransformType.Absolute);
        }

        private void OnCollision2DExitEvent(GameObject invoker, ref Collision2DExitEvent args)
        {
            if (!_triggered || !RevertTrigger) return;
            _triggered = false;

            Transform(Target, _lastPosition, _lastRotation, Duration, Ease, TransformType.Absolute);
        }

        public void Transform(GameObject go, Vector3 pdelta, Quaternion rdelta,
            float duration, bool ease, TransformType type)
        {
            if (Flag == TransformFlags.Position || Flag == TransformFlags.All)
                Move(go, pdelta, duration, ease, type);

            if (Flag == TransformFlags.Rotation || Flag == TransformFlags.All)
                Rotate(go, rdelta, duration, ease, type);
        }

        public void Move(GameObject go, Vector3 delta, float duration, bool ease, TransformType type)
            => StartCoroutine(IEMove(go, delta, duration, ease, type));

        private IEnumerator IEMove(GameObject go, Vector3 delta,
            float duration, bool ease, TransformType type)
        {
            if (go == null) yield break;

            var start = go.transform.localPosition;
            var end = type == TransformType.Delta ? start + delta : delta;

            if (duration <= 0f)
            {
                go.transform.localPosition = end;
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
                go.transform.localPosition = Vector3.Lerp(start, end, p);
            }

            // ensure exact final position
            go.transform.localPosition = end;
            yield break;
        }

        public void Rotate(GameObject go, Quaternion delta, float duration, bool ease, TransformType type)
            => StartCoroutine(IERotate(go, delta, duration, ease, type));

        private IEnumerator IERotate(GameObject go, Quaternion delta,
            float duration, bool ease, TransformType type)
        {
            if (go == null) yield break;

            var start = go.transform.localRotation;
            var end = type == TransformType.Delta ? start * delta : delta;

            if (duration <= 0f)
            {
                go.transform.localRotation = end;
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
                go.transform.localRotation = Quaternion.Slerp(start, end, p);
            }

            // ensure exact final rotation
            go.transform.localRotation = end;
            yield break;
        }
    }
}
