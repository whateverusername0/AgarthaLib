using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D
{
    public static class Extensions2D
    {
        public static void LookAt2D(this Transform t, Transform target)
            => t.up = t.LookRotation2D(target);

        public static void LookAt2D(this Transform t, Vector3 target)
            => t.up = t.LookRotation2D(target);

        public static Vector2 LookRotation2D(this Transform t, Transform target)
            => (target.position - t.position).normalized;

        public static Vector2 LookRotation2D(this Transform t, Vector3 target)
            => (target - t.position).normalized;

        #region Tilemaps

        public static readonly Vector3Int[] NeighborPositions = new Vector3Int[]
        {
            new(0, 1, 0), new(-1, 0, 0), new(1, 0, 0), new(0, -1, 0),
        };

        public static readonly Vector3Int[] NeighborPositionsDiagonal = new Vector3Int[]
        {
            new(-1, 1, 0),  new(0, 1, 0),  new(1, 1, 0),
            new(-1, 0, 0), new(1, 0, 0),
            new(-1, -1, 0), new(0, -1, 0), new(1, -1, 0)
        };

        public static Dictionary<Vector3Int, TileBase> GetAllTiles(this Tilemap tm, bool notNull = true)
        {
            var l = new Dictionary<Vector3Int, TileBase>();
            foreach (var pos in tm.cellBounds.allPositionsWithin)
            {
                var tile = tm.GetTile(pos);
                if (notNull && tile == null) continue;
                l.Add(pos, tile);
            }
            return l;
        }

        public static Dictionary<Vector3Int, T> GetAllTilesOfType<T>(this Tilemap tm) where T : TileBase
        {
            var d = tm.GetAllTiles(true).Where(q => q.Value.GetType() == typeof(T));
            return d.ToDictionary(q => q.Key, q => q.Value as T);
        }

        public static Dictionary<Vector3Int, TileBase> GetTilesInRange(this Tilemap tm,
            Vector3Int position, int range)
        {
            var l = new Dictionary<Vector3Int, TileBase>();
            for (int x = position.x - range; x <= position.x + range; x++)
                for (int y = position.y - range; y <= position.y + range; y++)
                    l.Add(new(x, y), tm.GetTile(new(x, y)));

            return l;
        }

        public static Dictionary<Vector3Int, TileBase> GetAdjacentTiles(this Tilemap tm,
            Vector3Int position, bool allowDiagonal)
        {
            var query = allowDiagonal ? NeighborPositionsDiagonal : NeighborPositions;
            var d = new Dictionary<Vector3Int, TileBase>();
            foreach (var pos in query)
            {
                var p = position + pos;
                var tile = tm.GetTile(p);
                if (tile == null) continue;
                d.Add(p, tile);
            }

            return d;
        }

        public static Dictionary<Vector3Int, TileBase> GetConnectedTiles(this Tilemap tm,
            Vector3Int position, TileBase tile, bool allowDiagonal)
        {
            var l = new Dictionary<Vector3Int, TileBase>();

            var startTile = tm.GetTile(position);
            var targetTile = tile != null ? tile : startTile;
            if (startTile != targetTile || startTile == null)
                return new();

            var queue = new Queue<Vector3Int>();
            var visited = new HashSet<Vector3Int>();

            queue.Enqueue(position);
            visited.Add(position);

            while (queue.Count > 0)
            {
                var currentPos = queue.Dequeue();
                var currentTile = tm.GetTile(currentPos);

                l.Add(currentPos, currentTile);

                foreach (var neighbor in tm.GetAdjacentTiles(currentPos, allowDiagonal))
                {
                    var neighborPos = neighbor.Key;

                    if (!tm.cellBounds.Contains(neighborPos)
                    || !visited.Add(neighborPos))
                        continue;

                    if (neighbor.Value == targetTile)
                        queue.Enqueue(neighborPos);
                }
            }

            return l;
        }

        #endregion
    }
}