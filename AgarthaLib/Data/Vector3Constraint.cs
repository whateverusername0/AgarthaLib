using System;

namespace AgarthaLib.Data
{
    [Serializable]
    public struct Vector3Constraint
    {
        public bool X, Y, Z;

        public Vector3Constraint(bool x = false, bool y = false, bool z = false)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}