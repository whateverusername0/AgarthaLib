using System;
using UnityEngine;

namespace AgarthaLib.Data
{
    [Serializable] public struct TransformData
    {
        public Vector3 Position, EulerAngles, Scale;
        public Quaternion Rotation;

        public TransformData(Vector3 position, Vector3 eulerAngles, Vector3 scale)
        {
            Position = position;
            EulerAngles = eulerAngles;
            Scale = scale;
            Rotation = Quaternion.Euler(eulerAngles);
        }

        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            EulerAngles = rotation.eulerAngles;
        }
    }
}
