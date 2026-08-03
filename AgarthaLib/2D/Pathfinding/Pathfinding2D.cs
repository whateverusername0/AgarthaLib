using AgarthaLib._2D.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Pathfinding
{
    [Serializable] public class Pathfinding2D<TGrid, TTilemap, TLayer>
        where TGrid : MapGrid<TTilemap, TLayer>
        where TTilemap : MapGridLayer<TLayer>
        where TLayer : Enum
    {
        public TGrid ActiveGrid;
        public TLayer ActiveLayer;

        public bool AllowDiagonalMovement = true;

        public Predicate<(Vector2Int position, TileBase tile)> WalkableTilePredicate = null;

        public Pathfinding2D(TLayer layer, TGrid grid)
        {
            ActiveLayer = layer;
            ActiveGrid = grid;
        }

        public bool IsWalkable(Vector2Int position, TileBase tile)
            => WalkableTilePredicate?.Invoke((position, tile)) ?? true;

        public bool TryFindPath(Vector2 start, Vector2 end, out List<Vector2> path)
        {
            path = FindPath(start, end);
            return path != null && path.Count > 0;
        }

        public bool TryFindPath(Vector2Int start, Vector2Int end, out List<Vector2Int> path)
        {
            path = FindPath(start, end);
            return path != null && path.Count > 0;
        }

        public List<Vector2> FindPath(Vector2 start, Vector2 end)
        {
            var grid = ActiveGrid;
            if (grid == null) return null;

            var startInt = grid.WorldToTile(ActiveLayer, start);
            var endInt = grid.WorldToTile(ActiveLayer, end);

            var path = FindPath(startInt, endInt);
            if (path == null || path.Count == 0)
                return null;

            var worldPath = new List<Vector2>(path.Count);
            for (int i = 0; i < path.Count; i++)
                worldPath[i] = grid.TileToWorld(ActiveLayer, path[i]);

            return worldPath;
        }

        public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
        {
            if (start == end) return new(); // lol

            var grid = ActiveGrid.GetTilemap(ActiveLayer);
            if (grid == null) return null;

            var endTile = grid.GetTile(end);
            if (!IsWalkable(end, endTile))
            {
                var area = grid.GetAdjacentTiles(end, AllowDiagonalMovement)
                    .Where(q => IsWalkable(q.position, q.tile)).ToList();
                if (area.Count == 0) return null;
                end = area.First().position;
            }

            var startNode = new Pathfinding2DNode(start);
            var startNodeWorld = grid.TileToWorld(start);

            var endNode = new Pathfinding2DNode(end);
            var endNodeWorld = grid.TileToWorld(end);

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
                var currentWorldPos = grid.TileToWorld(currentNode.Position);

                if (currentNode.Position == end)
                    return ReconstructPath(grid, currentNode);

                openSet.Remove(currentNode);
                closedSet.Add(currentNode.Position);

                foreach (var neighbor in grid.GetAdjacentTiles(currentNode.Position, AllowDiagonalMovement))
                {
                    var pos = neighbor.position;
                    var posTiles = grid.GetTile(pos);
                    if (closedSet.Contains(pos) || !IsWalkable(pos, posTiles))
                        continue;

                    var worldPos = grid.TileToWorld(pos);
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

        public List<Vector2> Truncate(List<Vector2> path)
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

        public List<Vector2Int> Truncate(List<Vector2Int> path)
        {
            var newPath = new List<Vector2Int>();
            var lastDelta = Vector2Int.zero;
            for (int i = 0; i < path.Count; i++)
            {
                if (i == 0) continue;
                var delta = path[i - 1] - path[i];

                if (delta != lastDelta)
                    newPath.Add(path[i - 1]);

                lastDelta = delta;
            }
            newPath.Add(path[^1]);

            return newPath.Distinct().ToList();
        }

        private Pathfinding2DNode GetLowestFCostNode(List<Pathfinding2DNode> nodes)
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

        private List<Vector2Int> ReconstructPath(TTilemap map, Pathfinding2DNode endNode)
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