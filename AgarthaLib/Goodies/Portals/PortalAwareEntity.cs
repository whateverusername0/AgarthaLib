using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Goodies.Portals
{
    /// <summary>
    ///     An entity that is supposed to be aware that a portal exists.
    ///     Modifies it's rotation accordingly to keep it on it's feet.
    /// </summary>
    public class PortalAwareEntity : AgarthanBehaviour
    {
        [ValidateNull] public Rigidbody RB;
        [ValidateNull] public CharacterController CC;
        public Transform Pivot;

        [Header("Rotation")]
        public bool RotateAutomatically = true;
        public float RotationSpeed = 2f;
        [EditorReadOnly] public Quaternion TargetRotation = Quaternion.identity;

        protected override void Start()
        {
            base.Start();

            SubscribeEvent<PortalTeleportedEvent>(OnPortalTeleported);
        }

        protected override void Update()
        {
            base.Update();

            if (RotateAutomatically)
            {
                var t = Pivot ? Pivot : transform;
                TargetRotation = Quaternion.Lerp(TargetRotation, GetWorldDirection(), Time.deltaTime * RotationSpeed);
                var tangles = TargetRotation.eulerAngles;
                t.eulerAngles = new(tangles.x, t.eulerAngles.y, tangles.z);
            }
        }

        // TODO gravity
        public Quaternion GetWorldDirection()
            => Quaternion.LookRotation(Vector3.forward, Vector3.up);

        private void OnPortalTeleported(GameObject invoker, ref PortalTeleportedEvent args)
        {
            var t = Pivot ? Pivot : transform;

            // look up again.
            TargetRotation = Quaternion.LookRotation(t.forward, Vector3.up);

            if (RB)
            {
                RB.velocity = args.Origin.TransformDirection(RB.velocity);
                RB.angularVelocity = args.Origin.TransformDirection(RB.angularVelocity);
                var velocityOverride = args.Destination.VelocityOverride;
                if (velocityOverride > 0)
                {
                    var currentVelocity = RB.velocity;
                    var newVelocity = args.Destination.OutwardsForward * velocityOverride;
                    RB.velocity = Vector3.Max(currentVelocity, newVelocity);
                }
            }

            if (CC)
            {
                Physics.SyncTransforms();
                // CharacterController controls velocity in their own logic.
            }
        }
    }
}
