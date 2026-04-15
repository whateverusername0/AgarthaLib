using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.EventSystem.StaticDispatchers
{
    public class DeviceInfoEventDispatcher : AgarthanSingleton<DeviceInfoEventDispatcher>
    {
        [SerializeField, EditorReadOnly] private Vector2 _resolution;
        public Vector2 Resolution => _resolution;

        protected override void Start()
        {
            base.Start();

            _resolution = new(Screen.width, Screen.height);
        }

        protected override void Update()
        {
            base.Update();

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
