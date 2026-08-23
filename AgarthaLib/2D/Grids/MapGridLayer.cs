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
        [EditorReadOnly, ValidateNull]
        public Tilemap Tilemap;

        [SerializeField, EditorReadOnly] protected TLayer _layer;

        public TLayer Layer => _layer;
        public virtual int GetLayerInt() => (int)(object)_layer;
        public virtual void SetLayer(TLayer layer) => _layer = layer;

        public abstract LayerData GetLayerData();

        public virtual void Clear()
            => Tilemap.ClearAllTiles();

        public virtual TileBase GetTile(Vector2Int pos)
            => Tilemap.GetTile(TransformPosition(pos));

        public virtual TTile GetTile<TTile>(Vector2Int pos) where TTile : class
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

        public virtual void RefreshTile(Vector2Int pos)
            => Tilemap.RefreshTile(TransformPosition(pos));

        public virtual Vector3Int TransformPosition(Vector2Int pos)
            => new(pos.x, pos.y, 0);

        public virtual Vector3 TransformPosition(Vector2 pos)
            => new(pos.x, pos.y, 0);

        public virtual Dictionary<Vector2Int, TileBase> GetAllTiles(bool notNull = true)
            => Tilemap.GetAllTiles(notNull)
            .ToDictionary(q => (Vector2Int)q.Key, q => q.Value);

        public virtual Dictionary<Vector2Int, T> GetAllTilesOfType<T>() where T : TileBase
            => Tilemap.GetAllTilesOfType<T>()
            .ToDictionary(q => (Vector2Int)q.Key, q => q.Value);

        public Dictionary<Vector2Int, TileBase> GetTilesInRange(Vector2Int position, int range)
            => Tilemap.GetTilesInRange((Vector3Int)position, range)
            .ToDictionary(q => (Vector2Int)q.Key, q => q.Value);

        public Dictionary<Vector2Int, TileBase> GetAdjacentTiles(Vector2Int position, bool allowDiagonal)
            => Tilemap.GetAdjacentTiles((Vector3Int)position, allowDiagonal)
            .ToDictionary(q => (Vector2Int)q.Key, q => q.Value);

        public Dictionary<Vector2Int, TileBase> GetConnectedTiles(Vector2Int position, TileBase tile, bool allowDiagonal)
            => Tilemap.GetConnectedTiles((Vector3Int)position, tile, allowDiagonal)
            .ToDictionary(q => (Vector2Int)q.Key, q => q.Value);
    }
}
