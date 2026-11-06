using AgarthaLib.Attributes;
using System;
using UnityEngine;

namespace AgarthaLib.Data
{
    /// <summary>
    ///     Simple wrapper to help reduce bloat of classes that should have cooldowns implemented.
    /// </summary>
    [Serializable] public class Cooldown
    {
        public float Delay = 1f;
        [EditorReadOnly] public float Timer = 0f;

        /// <param name="delay">The duration before the cooldown is considered complete</param>
        /// <param name="started">true to start the cooldown immediately</param>
        public Cooldown(float delay, bool started = false)
        {
            Delay = Mathf.Abs(delay);
            Timer = started ? delay : 0f;
        }

        /// <summary>
        ///     If the cooldown is available for use.
        /// </summary>
        public bool Available => Timer <= 0f;

        /// <summary>
        ///     Displays progress from 0 to 1 based on the timer.
        ///     Useful for progress bars and such.
        /// </summary>
        public float Difference
            => Timer == 0 ? 1 : 1 - (Timer / Delay);

        /// <summary>
        ///     Tries to start the cooldown.
        /// </summary>
        /// <returns>If it started</returns>
        public bool TryStartCooldown(bool force = false)
        {
            if (!Available && !force)
                return false;

            Timer = Delay;
            return true;
        }

        /// <summary>
        ///     Updates the timer with delta set to <see cref="Time.deltaTime"/>.
        /// </summary>
        public void UpdateCooldown()
            => UpdateCooldown(Time.deltaTime);

        /// <summary>
        ///     Subtracts delta from the timer and clamps it.
        /// </summary>
        /// <param name="delta">How much to subtract</param>
        public void UpdateCooldown(float delta)
            => Timer = Mathf.Clamp(Timer - delta, 0f, Delay);
    }
}
