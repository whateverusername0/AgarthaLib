using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Data
{
    [Serializable] public struct Box3D
    {
        public MinMaxInt X, Y, Z;

        public Box3D(MinMaxInt x, MinMaxInt y, MinMaxInt z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public readonly Vector3Int Min => new(X.Min, Y.Min, Z.Min);
        public readonly Vector3Int Max => new(X.Max, Y.Max, Z.Max);

        public static implicit operator Box2D(Box3D a) => new(a.X, a.Y);

        public readonly bool IsInBounds(Vector3Int position)
            => X.IsInBounds(position.x)
            && Y.IsInBounds(position.y)
            && Z.IsInBounds(position.z);

        public override readonly string ToString() => $"[({X}), ({Y}), ({Z})]";

        public readonly List<Vector3Int> ToArray()
        {
            var l = new List<Vector3Int>();
            for (int z = 0; z < Z.Length(); z++)
                for (int y = 0; y < Y.Length(); y++)
                    for (int x = 0; x < X.Length(); x++)
                        l.Add(new Vector3Int(x, y, z));
            return l;
        }
    }
}
