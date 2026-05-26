using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Goodies.Portals
{
    /// <summary>
    ///     An entity that is supposed to be aware of a portal.
    ///     Modifies it's rotation accordingly to keep it on it's feet.
    /// </summary>
    public class PortalAwareEntity : AgarthanBehaviour
    {
        [ValidateNull] public Rigidbody RB;
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
                var trot = Quaternion.Slerp(TargetRotation, GetUp(t), Time.deltaTime * RotationSpeed);

                TargetRotation = trot;
                var tangles = TargetRotation.eulerAngles;
                t.eulerAngles = new(tangles.x, t.eulerAngles.y, tangles.z);
            }
        }

        // TODO gravity
        public Quaternion GetUp(Transform t)
        {
            var plane = Vector3.ProjectOnPlane(t.forward, Vector3.up);
            if (plane == Vector3.zero)
                plane = t.forward;
            return Quaternion.LookRotation(plane, Vector3.up);
        }

        private void OnPortalTeleported(GameObject invoker, ref PortalTeleportedEvent args)
        {
            var t = Pivot ? Pivot : transform;

            // look up again.
            TargetRotation = args.NewRotation;

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

            Physics.SyncTransforms();
            // charactercontroller go
        }
    }
}
