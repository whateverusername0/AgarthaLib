using AgarthaLib._2D.Tilemaps;
using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Pathfinding
{
    public class Pathfinder2D : AgarthanBehaviour
    {
        public TilemapMap Map;

        [Header("Obstacles")]
        public bool MapObstacleDetection = true;
        public List<TileBase> ValidTiles = new();
        public bool AllowDiagonalMovement = true;

        public virtual bool IsWalkable(MapTileData data)
        {
            if (Map == null || data == null) return false;

            // walkable tiles exist and the tile's not there
            if (ValidTiles != null && ValidTiles.Count > 0
            && !ValidTiles.Any(q => q == data.Tile))
                return false;

            // not walkable according to the map
            if (MapObstacleDetection && !Map.IsWalkable(data))
                return false;

            return true;
        }

        public bool IsWalkable(Vector2Int position)
            => Map.GetTiles(position).All(IsWalkable);

        public bool TryFindPath(Vector2 start, Vector2 end, out List<Vector2> path)
            => Pathfinding2D.TryFindPath(Map, start, end, IsWalkable,
                MapObstacleDetection, AllowDiagonalMovement, out path);

        public bool TryFindPath(Vector2Int start, Vector2Int end, out List<Vector2Int> path)
            => Pathfinding2D.TryFindPath(Map, start, end, IsWalkable,
                MapObstacleDetection, AllowDiagonalMovement, out path);

        public List<Vector2> FindPath(Vector2 start, Vector2 end)
            => Pathfinding2D.FindPath(Map, start, end, IsWalkable,
                MapObstacleDetection, AllowDiagonalMovement);

        public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
            => Pathfinding2D.FindPath(Map, start, end, IsWalkable,
                MapObstacleDetection, AllowDiagonalMovement);
    }
}
