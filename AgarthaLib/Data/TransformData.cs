using System;
using UnityEngine;

namespace AgarthaLib.Data
{
    [Serializable] public struct TransformData
    {
        public Vector3 Position, EulerAngles, Scale;
        [NonSerialized] public Quaternion Rotation;

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

        public TransformData(Transform t) : this(t.localPosition, t.localEulerAngles, t.localScale)
        {
        }

        #region Helper Methods

        public static TransformData Lerp(TransformData a, TransformData b, float t)
        {
            var p = Vector3.Lerp(a.Position, b.Position, t);
            var r = Quaternion.Lerp(a.Rotation, b.Rotation, t);
            var s = Vector3.Lerp(a.Scale, b.Scale, t);
            return new(p, r, s);
        }

        #endregion
    }
}
