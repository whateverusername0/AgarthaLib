using System;
using UnityEngine;

namespace AgarthaLib._2D.Grids
{
    [Serializable] public enum Orientation : int
    {
        Up = 0,
        UpRight = 45,
        Right = 90,
        DownRight = 135,
        Down = 180,
        DownLeft = 225,
        Left = 270,
        UpLeft = 315,
    }

    public static class OrientationExtensions
    {
        public static int AsAngle(this Orientation o)
            => (int)o;

        public static Vector2Int AsVector2(this Orientation o)
        {
            return o switch
            {
                Orientation.Up => new(0, 1),
                Orientation.UpRight => new(1, 1),
                Orientation.Right => new(1, 0),
                Orientation.DownRight => new(1, -1),
                Orientation.Down => new(0, -1),
                Orientation.DownLeft => new(-1, -1),
                Orientation.Left => new(-1, 0),
                Orientation.UpLeft => new(-1, 1),
                _ => new(0, 0),
            };
        }
    }
}
