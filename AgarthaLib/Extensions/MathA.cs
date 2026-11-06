// kudos to https://blog.febucci.com/2018/08/easing-functions/ for easing functions
// also got cool graphs to look at them

using UnityEngine;

namespace AgarthaLib.Extensions
{
    /// <summary>
    ///     Math Agarthan. Contains not so obvious functions
    ///     that may not be found in other MathF libraries.
    /// </summary>
    public struct MathA
    {
        /// <summary>
        ///     Easing function. Takes t in range from 0f to 1f.
        /// </summary>
        /// <param name="t">
        ///     Float in range from 0 to 1
        /// </param>
        /// <returns>
        ///     Eased t based on the function.
        /// </returns>
        public static float EaseInQuadratic(float t)
            => Mathf.Pow(t, 2);

        /// <inheritdoc cref="EaseInQuadratic(float)"/>
        public static float EaseInCubic(float t)
            => Mathf.Pow(t, 3);

        /// <inheritdoc cref="EaseInQuadratic(float)"/>
        public static float EaseInExponential(float t)
            => t == 0f ? 0f : Mathf.Pow(2f, 10f * t - 10f);

        /// <inheritdoc cref="EaseInQuadratic(float)"/>
        public static float EaseOutQuadratic(float t)
            => 1f - Mathf.Pow(1f - t, 2f);

        /// <inheritdoc cref="EaseInQuadratic(float)"/>
        public static float EaseOutCubic(float t)
            => 1f - Mathf.Pow(1f - t, 3);

        /// <inheritdoc cref="EaseInQuadratic(float)"/>
        public static float EaseOutExponential(float t)
            => t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t);

        /// <summary>
        ///     1 - x. May be useful in some scenarios, I dunno.
        /// </summary>
        /// <param name="x"> x </param>
        /// <returns> 1 - x. </returns>
        public static float Flip(float x)
            => 1f - x;
    }
}
