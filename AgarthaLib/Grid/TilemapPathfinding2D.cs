using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Grid
{
    public class TilemapPathfinding2D : AgarthanBehaviour
    {
        public int MaxSeeks = 1024;

        public List<PathfindingNode> FoundNodes = new();
        public List<PathfindingNode> UnexploredNodes = new();
        public PathfindingNode CurrentNode;

        public MapDefinition Map;
        public Vector2 Destination;

        public bool AllowDiagonalMovement = true;
        public bool AllowCuttingCorners = false;

        /// <summary>
        ///     Setup the pathfinder for a new seek
        /// </summary>
        public void Set(MapDefinition grid, Vector2 destination, Vector2 start)
        {
            Map = grid;
            Destination = destination;
            CurrentNode = new (start);
            FoundNodes.Clear();
            UnexploredNodes.Clear();
        }

        /// <summary>
        ///     Seek a path from the start to the destination
        /// </summary>
        /// <returns>Path if possible, otherwise null</returns>
        public List<PathfindingNode> Seek()
        {
            var path = new List<PathfindingNode>();

            if (CurrentNode == null || Map == null || !Map.IsWalkable(Destination))
                return path;

            FoundNodes.Add(CurrentNode);

            for (int i = 0; i < MaxSeeks; i++)
            {
                if (CurrentNode.Position == Destination)
                {
                    path.Add(CurrentNode);
                    var node = CurrentNode.Parent;

                    while (node != null)
                    {
                        path.Add(node);
                        node = node.Parent;
                    }

                    // Since we iterate from the end,
                    // to the start we need to reverse the path to get a logical order.
                    path.Reverse();
                    break;
                }

                SeekNext();
            }

            return path;
        }

        /// <summary>
        ///     Seeks the the next highest priority node
        /// </summary>
        private void SeekNext()
        {
            var neighbours = GetNeighbours(CurrentNode);

            foreach (var neighbor in neighbours)
            {
                if (!Map.IsWalkable(neighbor.Position))
                    continue;

                var delta = neighbor.Position - Destination;
                delta = new Vector2(Math.Abs(delta.x), Math.Abs(delta.y));

                if (AllowDiagonalMovement && !AllowCuttingCorners
                && (!Map.IsWalkable(new Vector2(neighbor.Position.x, CurrentNode.Position.y))
                || !Map.IsWalkable(new Vector2(CurrentNode.Position.x, neighbor.Position.y))))
                    continue;

                // We have already found the neighbour
                if (FoundNodes.Contains(neighbor))
                {
                    // Check if the current path is shorter to the neighbour than the path previously found
                    // If so, update the already found
                    var foundNode = FoundNodes[FoundNodes.IndexOf(neighbor)];
                    if (neighbor.DistanceToStart < foundNode.DistanceToStart)
                    {
                        foundNode.Parent = neighbor.Parent;
                        foundNode.DistanceToStart = neighbor.DistanceToStart;
                    }

                    continue;
                }

                neighbor.DistanceToEnd = AllowDiagonalMovement
                    ? (float)Math.Sqrt(delta.x * delta.x + delta.y * delta.y) // Euclidean
                    : neighbor.DistanceToEnd = delta.x + delta.y; // Manhattan

                FoundNodes.Add(neighbor);
                UnexploredNodes.Add(neighbor);
            }

            // Sort the unexplored nodes to prioritize exploring nodes that have shortest distance to start + end
            UnexploredNodes.Sort((x, y) => x.DistanceToEnd + x.DistanceToStart < y.DistanceToEnd + y.DistanceToStart ? -1 : 1);

            // Pop the next node to explore
            CurrentNode = UnexploredNodes[0];
            UnexploredNodes.RemoveAt(0);
        }

        /// <summary>
        ///     Returns the neighbours to the node
        /// </summary>
        private List<PathfindingNode> GetNeighbours(PathfindingNode node)
        {
            float x = node.Position.x;
            float y = node.Position.y;
            float movementCost = node.DistanceToStart + 1f;

            var n = new List<PathfindingNode>
            {
                new (node, new Vector2(x, y + 1), movementCost), // top
				new (node, new Vector2(x + 1, y), movementCost), // right
				new (node, new Vector2(x, y - 1), movementCost), // bottom
				new (node, new Vector2(x - 1, y), movementCost)  // left
			};

            if (AllowDiagonalMovement)
            {
                float diagonalMovementCost = node.DistanceToStart + (float)Math.Sqrt(2);

                n.Add(new (node, new Vector2(x + 1, y + 1), diagonalMovementCost)); // top right
                n.Add(new (node, new Vector2(x + 1, y - 1), diagonalMovementCost)); // bottom right
                n.Add(new (node, new Vector2(x - 1, y - 1), diagonalMovementCost)); // bottom left
                n.Add(new (node, new Vector2(x - 1, y + 1), diagonalMovementCost)); // top left
            }

            return n;
        }

        /// <summary>
        ///     Compress a path to only contain significant nodes
        /// </summary>
        public static List<PathfindingNode> CompressPath(List<PathfindingNode> path)
        {
            // Nothing to compress
            if (path.Count < 3) return path;

            var compressedPath = new List<PathfindingNode>();

            var prevDirection = new Vector2(int.MaxValue, int.MaxValue);
            PathfindingNode prevNode = null;

            foreach (PathfindingNode node in path)
            {
                // Skip the first
                // It will automatically be added next iteration
                if (prevNode == null)
                {
                    prevNode = node;
                    continue;
                }

                var direction = node.Position - prevNode.Position;
                var directionDelta = prevDirection - direction;

                // Direction has changed, previous node is significant
                if (directionDelta.magnitude > 0)
                    compressedPath.Add(prevNode);

                prevDirection = direction;
                prevNode = node;
            }

            // Include the last node
            compressedPath.Add(path[path.Count - 1]);

            return compressedPath;
        }
    }

    [Serializable] public class PathfindingNode
    {
        public Vector2 Position;
        public float DistanceToEnd, DistanceToStart;
        public PathfindingNode Parent;

        public PathfindingNode(Vector2 position)
        {
            Position = position;
        }

        public PathfindingNode(PathfindingNode parent, Vector2 position, float distanceToStart) : this(position)
        {
            Parent = parent;
            DistanceToStart = distanceToStart;
        }

        public override bool Equals(object obj)
        {
            if (obj is PathfindingNode other) return this == other;
            else return base.Equals(obj);
        }

        public override int GetHashCode()
            => HashCode.Combine(Position.x, Position.y);
    }
}