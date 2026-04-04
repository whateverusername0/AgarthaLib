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
        public bool AutoResetRotation = false;
        [EditorReadOnly] public Quaternion TargetRotation = Quaternion.identity;

        protected override void Start()
        {
            base.Start();

            SubscribeEvent<PortalTeleportedEvent>(OnPortalTeleported);
        }

        private void OnPortalTeleported(GameObject invoker, ref PortalTeleportedEvent args)
        {
            var transform = Pivot ? Pivot : this.transform;

            // look up again.
            TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);

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
