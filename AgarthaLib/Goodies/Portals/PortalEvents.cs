using AgarthaLib.EventSystem;
using UnityEngine;

namespace AgarthaLib.Goodies.Portals
{
    public sealed class PortalTeleportedEvent : EventBase
    {
        public PortalRenderer Origin, Destination;
        public Vector3 NewPosition;
        public Quaternion NewRotation;

        public PortalTeleportedEvent(PortalRenderer origin, PortalRenderer destination, Vector3 newPosition, Quaternion newRotation)
        {
            Origin = origin;
            Destination = destination;
            NewPosition = newPosition;
            NewRotation = newRotation;
        }
    }
}
