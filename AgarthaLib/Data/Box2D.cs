using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Data
{
    [Serializable] public struct Box2D
    {
        public MinMaxInt X, Y;

        public Box2D(MinMaxInt x, MinMaxInt y)
        {
            X = x;
            Y = y;
        }

        public readonly Vector2Int Min => new(X.Min, Y.Min);
        public readonly Vector2Int Max => new(X.Max, Y.Max);

        public static implicit operator Box3D(Box2D a) => new(a.X, a.Y, MinMax.Zero);

        public readonly bool IsInBounds(Vector2Int position)
            => X.IsInBounds(position.x)
            && Y.IsInBounds(position.y);

        public override readonly string ToString() => $"[({X}), ({Y})]";

        public readonly List<Vector2Int> ToArray()
        {
            var l = new List<Vector2Int>();
            for (int y = 0; y < Y.Length(); y++)
                for (int x = 0; x < X.Length(); x++)
                    l.Add(new Vector2Int(x, y));
            return l;
        }
    }
}