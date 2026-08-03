using AgarthaLib._2D.Tilemaps;
using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Grids
{
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(TilemapRenderer))]
    public abstract class MapGridLayer<TLayer> : AgarthanBehaviour
        where TLayer : Enum
    {
        [SerializeField, EditorReadOnly, ValidateNull]
        public Tilemap Tilemap;

        [SerializeField, EditorReadOnly] protected TLayer _layer;

        public virtual int GetLayer() => (int)(object)_layer;
        public virtual void SetLayer(TLayer layer) => _layer = layer;

        public virtual MapGrid<MapGridLayer<TLayer>, TLayer> GetGrid()
            => GetComponentInParent<MapGrid<MapGridLayer<TLayer>, TLayer>>();

        public abstract LayerData GetLayerData();

        public virtual TileBase GetTile(Vector2Int pos)
            => Tilemap.GetTile(TransformPosition(pos));

        public virtual TTile GetTile<TTile>(Vector2Int pos)
            where TTile : TileBase
            => GetTile(pos) as TTile;

        public virtual bool TryGetTile(Vector2Int pos, out TileBase tile)
        {
            tile = GetTile(pos);
            return tile == null;
        }

        public virtual bool TryGetTile<TTile>(Vector2Int pos, out TTile ttile)
            where TTile : TileBase
        {
            ttile = null;
            if (!TryGetTile(pos, out var tile) || tile is not TTile)
                return false;

            ttile = tile as TTile;
            return tile;
        }

        public virtual bool TileExists(Vector2Int pos)
            => GetTile(pos) != null;

        public virtual Vector2Int WorldToTile(Vector2 pos)
            => (Vector2Int)Tilemap.WorldToCell(TransformPosition(pos));

        public virtual Vector2 TileToWorld(Vector2Int pos)
            => (Vector2)Tilemap.GetCellCenterWorld(TransformPosition(pos));

        public virtual void SetTile(Vector2Int pos, TileBase tile)
            => Tilemap.SetTile(TransformPosition(pos), tile);

        public virtual Vector3Int TransformPosition(Vector2Int pos)
            => new(pos.x, pos.y, GetLayer());

        public virtual Vector3 TransformPosition(Vector2 pos)
            => new(pos.x, pos.y, GetLayer());

        public List<(Vector2Int position, TileBase tile)> GetAllTiles(bool notNull = false)
        {
            var l = new List<(Vector2Int, TileBase)>();
            foreach (var pos in Tilemap.cellBounds.allPositionsWithin)
            {
                var tile = GetTile((Vector2Int)pos);
                if (notNull && tile == null) continue;
                l.Add(((Vector2Int)pos, tile));
            }
            return l;
        }

        public List<(Vector2Int position, T tile)> GetAllTilesOfType<T>() where T : TileBase
        {
            var d = GetAllTiles(true).Where(q => q.tile.GetType() == typeof(T));
            return (List<(Vector2Int, T)>)d.Select(q => (q.position, q.tile as T));
        }

        public List<(Vector2Int position, TileBase tile)> GetTilesInRange(Vector2Int position, int range)
        {
            var l = new List<(Vector2Int, TileBase)>();
            for (int x = position.x - range; x <= position.x + range; x++)
                for (int y = position.y - range; y <= position.y + range; y++)
                    l.Add((new(x, y), GetTile(new(x, y))));

            return l;
        }

        public List<(Vector2Int position, TileBase tile)> GetAdjacentTiles(Vector2Int position, bool allowDiagonal)
            => (List<(Vector2Int, TileBase)>)GetTilesInRange(position, 1)
            .Where(q => allowDiagonal || q.position.magnitude <= 1)
            .Where(q => q.position != position);

        public List<(Vector2Int position, TileBase tile)> GetConnectedTiles(Vector2Int position,
            TileBase tile, bool allowDiagonal)
        {
            var l = new List<(Vector2Int, TileBase)>();

            var startTile = GetTile(position);
            var targetTile = tile != null ? tile : startTile;
            if (startTile != targetTile || startTile == null)
                return new();

            var queue = new Queue<Vector2Int>();
            var visited = new HashSet<Vector2Int>();

            queue.Enqueue(position);
            visited.Add(position);

            while (queue.Count > 0)
            {
                var currentPos = queue.Dequeue();
                var currentTile = GetTile(currentPos);

                l.Add((currentPos, currentTile));

                foreach (var neighbor in GetAdjacentTiles(currentPos, allowDiagonal))
                {
                    var neighborPos = neighbor.position;

                    if (!Tilemap.cellBounds.Contains(TransformPosition(neighborPos))
                    || !visited.Add(neighborPos))
                        continue;

                    if (neighbor.tile == targetTile)
                        queue.Enqueue(neighborPos);
                }
            }

            return l;
        }
    }
}
