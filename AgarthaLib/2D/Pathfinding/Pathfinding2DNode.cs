using System;
using UnityEngine;

namespace AgarthaLib._2D.Pathfinding
{
    [Serializable] public class Pathfinding2DNode
    {
        public Pathfinding2DNode Parent;
        public Vector2Int Position;

        public float GCost = float.MaxValue;
        public float HCost = 0f;

        public float FCost => GCost + HCost;

        public Pathfinding2DNode(Vector2Int pos)
            => Position = pos;
    }
}
