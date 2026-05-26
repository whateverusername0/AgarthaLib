using UnityEngine;

namespace AgarthaLib.Extensions
{
    public static class QuaternionExtensions
    {
        public static Quaternion Lerp(this Quaternion a, Quaternion b, float t, float threshold)
        {
            var lerp = Quaternion.Lerp(a, b, t);
            if (Quaternion.Angle(lerp, b) <= threshold)
                return b;
            return lerp;
        }
    }
}
