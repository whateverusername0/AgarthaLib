using AgarthaLib.Attributes;
using AgarthaLib.Collision;
using AgarthaLib.Data.Serialization.SerializedTypes;
using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace AgarthaLib.Goodies.Portals
{
    public abstract class PortalBase : AgarthanBehaviour
    {
        [Header("Properties")]
        public PortalBase LinkedPortal;
        public Transform NormalVisible;
        public Transform NormalInvisible;

        public Vector3 InwardsForward => NormalVisible.forward;
        public Vector3 OutwardsForward => NormalInvisible.forward;

        [Header("Collision")]
        public bool CanPassThrough = true;
        public float VelocityOverride = 1.5f;
        public List<SerializedType> PassthroughTypes = new()
        {
            typeof(Rigidbody),
            typeof(Rigidbody2D),
            typeof(CharacterController),
            typeof(NavMeshAgent)
        };
        [SerializeField, EditorReadOnly] private List<Transform> _collidingObjects = new();
        [SerializeField, EditorReadOnly] private List<Transform> _objectRemovalQueue = new();

        protected override void Start()
        {
            base.Start();

            SubscribeEvent<CollisionEnterEvent>(OnCollisionEnterEvent);
            SubscribeEvent<CollisionExitEvent>(OnCollisionExitEvent);
        }

        private void OnCollisionEnterEvent(GameObject invoker, ref CollisionEnterEvent args)
        {
            var other = args.Other.transform.root;
            var compRegistry = other.GetComponents<Component>();
            // check if it has any valid components for it to pass through.
            if (compRegistry.Where(q => PassthroughTypes.Any(w => q.GetType() == w.Type)).Count() == 0)
                return;

            // entering the portal from behind - don't add
            if (GetDotProduct(other.position) < 0f)
                return;

            if (!_collidingObjects.Contains(other))
                _collidingObjects.Add(other);
        }

        private void OnCollisionExitEvent(GameObject invoker, ref CollisionExitEvent args)
        {
            var other = args.Other.transform.root;
            if (_collidingObjects.Contains(other))
                _collidingObjects.Remove(other);
        }

        protected override void LateFixedUpdate()
        {
            base.LateFixedUpdate();

            if (LinkedPortal == null || !CanPassThrough)
                return;

            _objectRemovalQueue.Clear();
            foreach (var item in _collidingObjects)
            {
                if (item == null)
                {
                    _objectRemovalQueue.Add(item);
                    continue;
                }

                // > 0 - not entered yet
                if (GetDotProduct(item.position) > 0)
                    continue;

                var newPos = TransformPosition(item.position);
                var newRot = TransformRotation(item.rotation);
                item.SetPositionAndRotation(newPos, newRot);

                RaiseEvent<PortalTeleportedEvent>(item.gameObject, new(this, LinkedPortal, newPos, newRot));

                _objectRemovalQueue.Add(item);
            }

            foreach (var item in _objectRemovalQueue)
                _collidingObjects.Remove(item);
        }

        protected virtual void OnDrawGizmos()
        {
            // forward + up arrows

            // facing the player
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(NormalVisible.position, NormalVisible.position + NormalVisible.forward);
            Gizmos.DrawLine(NormalVisible.position, NormalVisible.position + NormalVisible.up);

            // facing the exit
            Gizmos.color = Color.red;
            Gizmos.DrawLine(NormalInvisible.position, NormalInvisible.position + NormalInvisible.forward);
            Gizmos.DrawLine(NormalInvisible.position, NormalInvisible.position + NormalInvisible.up);

            // link
            Gizmos.color = Color.yellow;
            if (LinkedPortal != null && LinkedPortal != this)
                Gizmos.DrawLine(transform.position, LinkedPortal.transform.position);
        }

        #region API

        public static Vector3 TransformPosition(PortalBase a, PortalBase b, Vector3 position)
            => b.NormalInvisible.TransformPoint(a.NormalVisible.InverseTransformPoint(position));

        public Vector3 TransformPosition(Vector3 position)
            => TransformPosition(this, LinkedPortal, position);

        public static Vector3 TransformDirection(PortalBase a, PortalBase b, Vector3 direction)
            => b.NormalInvisible.TransformDirection(a.NormalVisible.InverseTransformDirection(direction));

        public Vector3 TransformDirection(Vector3 direction)
            => TransformDirection(this, LinkedPortal, direction);

        public Quaternion GetRotationDelta(PortalBase a, PortalBase b)
            => b.NormalInvisible.rotation * Quaternion.Inverse(a.NormalVisible.rotation);

        public Quaternion GetRotationDelta()
            => GetRotationDelta(this, LinkedPortal);

        public static Quaternion TransformRotation(PortalBase a, PortalBase b, Quaternion rotation)
            => b.NormalInvisible.rotation * Quaternion.Inverse(a.NormalVisible.rotation) * rotation;

        public Quaternion TransformRotation(Quaternion rotation)
            => TransformRotation(this, LinkedPortal, rotation);

        public float GetDotProduct(Vector3 position)
        {
            var direction = (position - transform.position).normalized;
            var dot = Vector3.Dot(direction, NormalVisible.forward);
            return dot;
        }

        #endregion
    }
}
