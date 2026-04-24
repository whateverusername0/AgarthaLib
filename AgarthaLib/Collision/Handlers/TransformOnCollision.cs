using AgarthaLib.Attributes;
using AgarthaLib.EventSystem;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AgarthaLib.Collision.Handlers
{
    public class TransformOnCollision : AgarthanBehaviour
    {
        [Header("Properties")]
        [SerializeField] private Transform _target;
        public Transform Target => _target != null ? _target : this.transform;

        [SerializeField] private TransformFlags Flag = TransformFlags.All;
        [SerializeField] private TransformType Type = TransformType.Delta;
        public Vector3 Position = Vector3.zero;
        public Quaternion Rotation = Quaternion.identity;
        
        [Header("Timing")]
        public float Duration = 1f;
        public bool Ease = true;

        [Header("Trigger")]
        public bool RevertTrigger = false;
        [SerializeField] private UnityEvent _beginEvent;
        [SerializeField] private UnityEvent _endEvent;
        [SerializeField] private UnityEvent _revertBeginEvent;
        [SerializeField] private UnityEvent _revertEndEvent;

        [Header("Private")]
        [SerializeField, EditorReadOnly] private Vector3 _lastPosition;
        [SerializeField, EditorReadOnly] private Quaternion _lastRotation;
        [SerializeField, EditorReadOnly] private bool _triggered = false;

        protected override void Start()
        {
            base.Start();

            _lastPosition = Target.localPosition;
            _lastRotation = Target.localRotation;

            SubscribeEvent<CollisionEnterEvent>(OnCollisionEnterEvent);
            SubscribeEvent<Collision2DEnterEvent>(OnCollision2DEnterEvent);
            SubscribeEvent<CollisionExitEvent>(OnCollisionExitEvent);
            SubscribeEvent<Collision2DExitEvent>(OnCollision2DExitEvent);
            SubscribeEvent<RelayedEvent<CollisionEnterEvent>>(OnRelayedCollisionEnterEvent);
            SubscribeEvent<RelayedEvent<Collision2DEnterEvent>>(OnRelayedCollision2DEnterEvent);
            SubscribeEvent<RelayedEvent<CollisionExitEvent>>(OnRelayedCollisionExitEvent);
            SubscribeEvent<RelayedEvent<Collision2DExitEvent>>(OnRelayedCollision2DExitEvent);
        }

        private void OnDrawGizmosSelected()
        {
            if (Target == null) return;

            Gizmos.color = Color.red;
            var t = Target;
            Gizmos.DrawRay(t.position, Position);
            if (Rotation.eulerAngles.magnitude > 0)
                Gizmos.DrawRay(t.position + t.forward, Rotation * t.forward);
        }

        private void OnCollisionEnterEvent(GameObject invoker, ref CollisionEnterEvent args)
            => CollisionEnter();

        private void OnCollision2DEnterEvent(GameObject invoker, ref Collision2DEnterEvent args)
            => CollisionEnter();

        private void OnCollisionExitEvent(GameObject invoker, ref CollisionExitEvent args)
            => CollisionExit();

        private void OnCollision2DExitEvent(GameObject invoker, ref Collision2DExitEvent args)
            => CollisionExit();

        private void OnRelayedCollisionEnterEvent(GameObject invoker, ref RelayedEvent<CollisionEnterEvent> args)
            => CollisionEnter();

        private void OnRelayedCollision2DEnterEvent(GameObject invoker, ref RelayedEvent<Collision2DEnterEvent> args)
            => CollisionEnter();

        private void OnRelayedCollisionExitEvent(GameObject invoker, ref RelayedEvent<CollisionExitEvent> args)
            => CollisionExit();

        private void OnRelayedCollision2DExitEvent(GameObject invoker, ref RelayedEvent<Collision2DExitEvent> args)
            => CollisionExit();

        private void CollisionEnter()
        {
            if (_triggered) return;
            _triggered = true;

            var pos = Type == TransformType.Delta ? _lastPosition + Position : Position;
            var rot = Type == TransformType.Delta ? _lastRotation * Rotation : Rotation;
            Transform(Target, pos, rot, Duration, Ease, TransformType.Absolute);
        }

        private void CollisionExit()
        {
            if (!_triggered || !RevertTrigger) return;
            _triggered = false;

            Transform(Target, _lastPosition, _lastRotation, Duration, Ease, TransformType.Absolute);
        }

        public void Transform(Transform t, Vector3 pdelta, Quaternion rdelta, float duration, bool ease, TransformType type)
        {
            if (Flag == TransformFlags.Position || Flag == TransformFlags.All)
                t.SmoothMoveCoroutine(pdelta, duration, ease, type);

            if (Flag == TransformFlags.Rotation || Flag == TransformFlags.All)
                t.SmoothRotateCoroutine(rdelta, duration, ease, type);
        }
    }
}
