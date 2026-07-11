using AgarthaLib._2D.Tilemaps;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib._2D.Pathfinding
{
    public static class Pathfinding2D
    {
        public static bool TryFindPath(TilemapMap map, Vector2 start, Vector2 end,
            Predicate<MapTileData> isWalkable, bool inclusive, out List<Vector2> path)
        {
            path = FindPath(map, start, end, isWalkable, inclusive);
            return path != null && path.Count > 0;
        }

        public static bool TryFindPath(TilemapMap map, Vector2Int start, Vector2Int end,
            Predicate<MapTileData> isWalkable, bool inclusive, out List<Vector2Int> path)
        {
            path = FindPath(map, start, end, isWalkable, inclusive);
            return path != null && path.Count > 0;
        }

        public static List<Vector2> FindPath(TilemapMap map, Vector2 start, Vector2 end,
            Predicate<MapTileData> isWalkable, bool inclusive)
        {
            var startInt = map.WorldToCell(start);
            var endInt = map.WorldToCell(end);

            var path = FindPath(map, startInt, endInt, isWalkable, inclusive);
            if (path == null || path.Count == 0)
                return null;

            var worldPath = new List<Vector2>(path.Count);
            for (int i = 0; i < path.Count; i++)
                worldPath[i] = map.CellToWorld(path[i]);

            return worldPath;
        }

        public static List<Vector2Int> FindPath(TilemapMap map, Vector2Int start, Vector2Int end,
            Predicate<MapTileData> isWalkable, bool inclusive)
        {
            var endTiles = map.GetTiles(end);
            if (inclusive ? endTiles.Any(q => !isWalkable(q)) : endTiles.All(q => !isWalkable(q)))
            {
                var area = map.GetAdjacentTiles(end).Where(q => isWalkable(q)).ToList();
                if (area.Count == 0) return null;
                end = (Vector2Int)area.First().Position;
            }

            var startNode = new Pathfinding2DNode(start);
            var startNodeWorld = map.CellToWorld(start);

            var endNode = new Pathfinding2DNode(end);
            var endNodeWorld = map.CellToWorld(end);

            var openSet = new List<Pathfinding2DNode>() { startNode };
            var closedSet = new HashSet<Vector2Int>();

            var nodes = new Dictionary<Vector2Int, Pathfinding2DNode>
            {
                { start, startNode }
            };

            startNode.GCost = 0;
            startNode.HCost = Vector2.Distance(startNodeWorld, endNodeWorld);

            while (openSet.Count > 0)
            {
                var currentNode = GetLowestFCostNode(openSet);
                var currentWorldPos = map.CellToWorld(currentNode.Position);

                if (currentNode.Position == end)
                    return ReconstructPath(map, currentNode);

                openSet.Remove(currentNode);
                closedSet.Add(currentNode.Position);

                foreach (var neighbor in map.GetAdjacentTiles(currentNode.Position))
                {
                    var pos = (Vector2Int)neighbor.Position;
                    var posTiles = map.GetTiles(pos);
                    if (closedSet.Contains(pos)
                    || inclusive ? posTiles.Any(q => !isWalkable(q)) : posTiles.All(q => !isWalkable(q)))
                        continue;

                    var worldPos = map.CellToWorld(pos);
                    var tg = currentNode.GCost + Vector2.Distance(currentWorldPos, worldPos);

                    if (!nodes.TryGetValue(pos, out var nNode))
                    {
                        nNode = new Pathfinding2DNode(pos);
                        nodes[pos] = nNode;
                    }

                    if (tg < nNode.GCost || !openSet.Contains(nNode))
                    {
                        nNode.GCost = tg;
                        nNode.HCost = Vector2.Distance(worldPos, endNodeWorld);
                        nNode.Parent = currentNode;

                        if (!openSet.Contains(nNode))
                            openSet.Add(nNode);
                    }
                }
            }

            return null;
        }

        public static List<Vector2> Truncate(List<Vector2> path)
        {
            var newPath = new List<Vector2>();
            var lastDelta = Vector2.zero;
            for (int i = 0; i < path.Count; i++)
            {
                if (i == 0)
                {
                    newPath.Add(path[i]);
                    continue;
                }
                var delta = path[i - 1] - path[i];

                if (delta != lastDelta)
                    newPath.Add(path[i - 1]);

                lastDelta = delta;
            }
            newPath.Add(path[^1]);
            return newPath;
        }

        public static List<Vector2Int> Truncate(List<Vector2Int> path)
        {
            var newPath = new List<Vector2Int>();
            var lastDelta = Vector2Int.zero;
            for (int i = 0; i < path.Count; i++)
            {
                if (i == 0)
                {
                    newPath.Add(path[i]);
                    continue;
                }
                var delta = path[i - 1] - path[i];

                if (delta != lastDelta)
                    newPath.Add(path[i - 1]);

                lastDelta = delta;
            }
            newPath.Add(path[^1]);

            return newPath;
        }

        private static Pathfinding2DNode GetLowestFCostNode(List<Pathfinding2DNode> nodes)
        {
            var best = nodes[0];
            foreach (var node in nodes)
            {
                if (node.FCost < best.FCost
                || (node.FCost == best.FCost && node.HCost < best.HCost))
                    best = node;
            }
            return best;
        }

        private static List<Vector2Int> ReconstructPath(TilemapMap map, Pathfinding2DNode endNode)
        {
            var path = new List<Vector2Int>();
            var current = endNode;
            while (current != null)
            {
                path.Add(current.Position);
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }
    }
}