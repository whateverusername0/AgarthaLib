using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

#if USING_TILEMAP_EXTRAS
using AgarthaLib._2D.Tilemaps.RuleTiles;
#endif

namespace AgarthaLib._2D.Tilemaps
{
    /// <summary>
    ///     Combines and represents multiple tilemaps as a single map. Also provides helper methods.
    /// </summary>
    /// <remarks>
    ///     Multiple tilemaps are considered multiple Z layers that are combined on Start.
    /// </remarks>
    public class TilemapMap : AgarthanBehaviour
    {
        [Header("Tilemap")]
        public List<Tilemap> Layers;
        public Tilemap Map;

        [Header("Bounds")]
        public BoundsInt Bounds;
        public bool DefineBoundsOnStart = false;

        protected override void Start()
        {
            base.Start();

            if (Map == null)
            {
                var mt = new GameObject(nameof(TilemapMap));
                mt.transform.SetParent(this.transform, false);
                Map = mt.AddComponent<Tilemap>();
                var mtr = mt.AddComponent<TilemapRenderer>();
                mtr.sortingOrder = -1;
            }

            if (Layers != null && Layers.Count > 0)
            {
                var snapshot = new List<Tilemap>(Layers);
                Layers.Clear();

                for (int i = 0; i < snapshot.Count; i++)
                {
                    MergeLayer(snapshot[i], i);
                    Destroy(snapshot[i].gameObject);
                }
            }
        }

        private void MergeLayer(Tilemap layer, int z = 0)
        {
            if (Map == null) return;

            foreach (var pos in layer.cellBounds.allPositionsWithin)
            {
                if (!TryGetTile(pos, out var tile))
                    continue;

                var newPos = new Vector3Int(pos.x, pos.y, z);
                Map.SetTile(newPos, tile);
            }
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(Bounds.position, Bounds.size);
        }

        public MapTileData GetTile(Vector3Int position)
        {
            var tile = Map.GetTile(position);
            if (tile != null) return new MapTileData(Map, position, tile);
            else return null;
        }

        public bool TryGetTile(Vector3Int position, out MapTileData tile)
        {
            tile = GetTile(position);
            return tile != null;
        }

        public List<MapTileData> GetTiles(Vector2Int position)
        {
            var list = new List<MapTileData>();
            for (int i = Map.cellBounds.zMin; i <= Map.cellBounds.zMax; i++)
            {
                var pos = new Vector3Int(position.x, position.y, i);
                var tile = GetTile(pos);
                if (tile != null) list.Add(tile);
            }
            return list;
        }

        public void SetTile(Vector3Int position, TileBase tile)
        {
            if (!Bounds.Contains(position))
            {
                Debug.LogError($"Tile with position of {position} is out of bounds {Bounds}.");
                return;
            }

            // in favor of scriptable objects
            var existing = GetTile(position);
            if (existing != null) Destroy(existing.Tile);
            if (tile != null) tile = Instantiate(tile);

            Map.SetTile(position, tile);
            Map.RefreshTile(position);
        }

        public List<MapTileData> GetTilesInRange(Vector2Int position, int range)
        {
            var list = new List<MapTileData>();
            for (int x = position.x - range; x <= position.x + range; x++)
                for (int y = position.y - range; y <= position.y + range; y++)
                    list.AddRange(GetTiles(new Vector2Int(x, y)));
            return list;
        }

        public List<MapTileData> GetAdjacentTiles(Vector2Int position, int range = 1)
            => GetTilesInRange(position, range)
            .Where(q => new Vector2Int(q.Position.x, q.Position.y) != position)
            .ToList();

        public List<MapTileData> GetAdjacentTiles(Vector3Int position)
            => GetAdjacentTiles(new Vector2Int(position.x, position.y));

        public List<MapTileData> GetAllPossibleTiles()
        {
            var list = new List<MapTileData>();
            foreach (var pos in Map.cellBounds.allPositionsWithin)
            {
                var tile = Map.GetTile(pos);
                if (tile != null) list.Add(new MapTileData(Map, pos, tile));
            }
            return list;
        }

        #region Extensions

        public Vector3Int WorldToCell(Vector3 position)
            => Map.WorldToCell(position);

        public Vector3 CellToWorld(Vector3Int position)
            => Map.GetCellCenterWorld(position);

        public Vector2Int WorldToCell(Vector2 position)
            => (Vector2Int)Map.WorldToCell(position);

        public Vector2 CellToWorld(Vector2Int position)
            => Map.GetCellCenterWorld((Vector3Int)position);

        public bool IsWalkable(MapTileData data)
        {
            var isNull = data == null || data.Tile == null;
            var noCollision = data.Tile is Tile t && t.colliderType == Tile.ColliderType.None;

            #if USING_TILEMAP_EXTRAS
            var noAgarthanCollision = data.Tile is AgarthanTileBase { } art && !art.ProvidesCollision;
            return !isNull && (noCollision || noAgarthanCollision);
            #else
            return !isNull && noCollision;
            #endif
        }

        public bool IsWalkable(Vector2Int position)
            => GetTiles(position).All(q => IsWalkable(q));

        #endregion
    }
}
