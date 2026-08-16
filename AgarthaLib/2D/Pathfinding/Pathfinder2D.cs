using AgarthaLib._2D.Grids;
using AgarthaLib.Attributes;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Pathfinding
{
    public abstract class Pathfinder2D<TGrid, TTilemap, TLayer> : AgarthanBehaviour
        where TGrid : MapGrid<TTilemap, TLayer>
        where TTilemap : MapGridLayer<TLayer>
        where TLayer : Enum
    {
        private Pathfinding2D<TGrid, TTilemap, TLayer> _pathfinder;

        [Header("Obstacles")]
        public bool UseValidTiles = false;
        public List<TileBase> ValidTiles = new();
        public bool AllowDiagonalMovement = true;

        public Pathfinding2D<TGrid, TTilemap, TLayer> PathfindingProvider => _pathfinder;

        public virtual bool IsWalkable(TGrid grid, TLayer activeLayer, Vector2Int pos, TileBase tile)
        {
            // grid does not exist in there
            if (grid == null || grid.GetTilemap(activeLayer) == null)
                return false;

            // not a valid tile to walk on
            if (UseValidTiles
            && ValidTiles != null && ValidTiles.Count > 0
            && !ValidTiles.Contains(tile))
                return false;

            var tiles = grid.GetAllTilesOn(pos)
                .Where(q => (int)(object)q.layer > (int)(object)activeLayer)
                .ToList();

            // check for obstructions
            // TODO make multi layer check
            var layerData = grid.GetLayerData(activeLayer);
            foreach (var potentialObstable in tiles)
            {
                if (layerData == null)
                    break;

                var poLayerData = grid.GetLayerData(potentialObstable.layer);
                if (poLayerData == null)
                    continue;

                // basically if the layers collide
                if (poLayerData.ProvidesCollision
                && layerData.CollisionLayer.IsInLayerMask(poLayerData.CollisionLayer))
                    return false;
            }

            return true;
        }

        public Pathfinding2D<TGrid, TTilemap, TLayer> GetPathfinder(TGrid grid, TLayer activeLayer)
        {
            _pathfinder ??= new(activeLayer, grid);
            _pathfinder.ActiveLayer = activeLayer;
            _pathfinder.ActiveGrid = grid;
            _pathfinder.AllowDiagonalMovement = AllowDiagonalMovement;
            _pathfinder.WalkableTilePredicate = (q) => IsWalkable(grid, activeLayer, q.position, q.tile);

            return _pathfinder;
        }

        public bool TryFindPath(TGrid grid, TLayer activeLayer,
            Vector2 start, Vector2 end, out List<Vector2> path)
            => GetPathfinder(grid, activeLayer).TryFindPath(start, end, out path);

        public bool TryFindPath(TGrid grid, TLayer activeLayer,
            Vector2Int start, Vector2Int end, out List<Vector2Int> path)
            => GetPathfinder(grid, activeLayer).TryFindPath(start, end, out path);

        public List<Vector2> FindPath(TGrid grid, TLayer activeLayer,
            Vector2 start, Vector2 end)
            => GetPathfinder(grid, activeLayer).FindPath(start, end);

        public List<Vector2Int> FindPath(TGrid grid,
            TLayer activeLayer, Vector2Int start, Vector2Int end)
            => GetPathfinder(grid, activeLayer).FindPath(start, end);
    }
}
