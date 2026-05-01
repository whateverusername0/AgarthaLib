using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Goodies.Pause
{
    public class PauseManager : AgarthanSingleton<PauseManager>
    {
        [Header("Properties")]
        public bool Paused = false;
        public bool CanPause = true;
        public bool CanUnpause = true;

        [Header("Conditionals")]
        public bool PauseOnFocus = true;

        protected override void Start()
        {
            base.Start();

            TogglePause(false, forced: true);

            CanPause = true;
            CanUnpause = true;
        }

        protected override void Update()
        {
            base.Update();

            if (Paused) { Pause(); return; }
            else Unpause();

            if (PauseOnFocus)
                TogglePause(!Application.isFocused);
        }

        public bool TogglePause()
            => TogglePause(!Paused);

        public bool TogglePause(bool paused, bool forced = false)
        {
            if (paused && (CanPause || forced))
            {
                Paused = true;
                return true;
            }
            else if (!paused && (CanUnpause || forced))
            {
                Paused = false;
                return true;
            }

            return false;
        }

        public void Pause()
        {
            Time.timeScale = 0f;
        }

        public void Unpause()
        {
            Time.timeScale = 1f;
        }
    }
}