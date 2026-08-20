using System;
using UnityEngine;

namespace AgarthaLib._2D.Grids
{
    [Serializable] public enum Direction : int
    {
        North = 0,
        East = 90,
        South = 180,
        West = 270,
        Any = 360,
    }

    public static class OrientationExtensions
    {
        public static int AsAngle(this Direction o)
            => (int)o;

        public static Vector2Int AsVector2(this Direction o)
        {
            return o switch
            {
                Direction.North => new(0, 1),
                Direction.East => new(1, 0),
                Direction.South => new(0, -1),
                Direction.West => new(-1, 0),
                Direction.Any => new(0, 0),
                _ => new(0, 0),
            };
        }

        public static Direction Reverse(this Direction o)
        {
            return o switch
            {
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.East => Direction.West,
                Direction.West => Direction.East,
                Direction.Any => Direction.Any,
                _ => Direction.Any
            };
        }
    }
}
