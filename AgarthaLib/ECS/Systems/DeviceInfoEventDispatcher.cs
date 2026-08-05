using AgarthaLib.Attributes;
using AgarthaLib.EventSystem;
using UnityEngine;

namespace AgarthaLib.ECS.Systems
{
    /// <summary>
    ///     Raises events based on device data being changed such as screen resolution.
    /// </summary>
    public class DeviceInfoEventDispatcher : EntitySystem
    {
        [SerializeField, EditorReadOnly] private Vector2 _resolution;
        public Vector2 Resolution => _resolution;

        public override void Initialize()
        {
            _resolution = new(Screen.width, Screen.height);
        }

        public override void UpdateSystem()
        {
            var newRes = new Vector2(Screen.width, Screen.height);
            if (_resolution != newRes)
            {
                RaiseGlobalEvent(new ResolutionChangedEvent(_resolution, newRes));
                _resolution = newRes;
            }
        }
    }

    public sealed class ResolutionChangedEvent : EventBase
    {
        public Vector2 OldResolution;
        public Vector2 NewResolution;

        public ResolutionChangedEvent(Vector2 oldResolution, Vector2 newResolution)
        {
            OldResolution = oldResolution;
            NewResolution = newResolution;
        }
    }
}
