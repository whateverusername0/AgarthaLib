using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Goodies.Pause
{
    public abstract class PausedSingleton<T> : AgarthanSingleton<T> where T : PausedSingleton<T>
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

        protected virtual void LateUpdate()
        {
            if (_pause.Paused && !UpdateOnPause)
                return;

            UnpausedLateUpdate();
        }

        protected virtual void UnpausedLateUpdate() { }
    }
}