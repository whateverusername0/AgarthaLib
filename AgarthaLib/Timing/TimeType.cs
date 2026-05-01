using System;

namespace AgarthaLib.Timing
{
    [Serializable] public enum TimeType
    {
        Normal,
        Late,
        Unscaled,
        LateUnscaled,
        Fixed,
        FixedUnscaled,
    }
}
