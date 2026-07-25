using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Goodies.Pause
{
    public abstract class PausedBehavior : AgarthanBehaviour
    {
        protected PauseManager _pause => PauseManager.Instance;

        [Header("Paused Behavior")]
        public bool UpdateOnPause = false;

        protected override void Update()
        {
            base.Update();

            if (_pause.Paused && !UpdateOnPause)
                return;

            UnpausedUpdate();
        }

        protected virtual void UnpausedUpdate() { }

        protected virtual void FixedUpdate()
        {
            if (_pause.Paused && !UpdateOnPause)
                return;

            UnpausedFixedUpdate();
        }

        protected virtual void UnpausedFixedUpdate() { }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (_pause.Paused && !UpdateOnPause)
                return;

            UnpausedLateUpdate();
        }

        protected virtual void UnpausedLateUpdate() { }
    }
}