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
        public Grid Grid;
        public List<Tilemap> Layers = new();
        public Tilemap MergedMap;

        public bool IsMerged => MergedMap != null;
        public bool IsLayered => Layers != null && Layers.Count > 0;

        [Header("Bounds")]
        public BoundsInt Bounds;
        public bool DefineBoundsOnStart = false;

        protected override void Start()
        {
            base.Start();

            if (DefineBoundsOnStart)
                Bounds = GetMap(0).cellBounds;
        }

        [ContextMenu("Merge layers")]
        private void MergeLayers()
        {
            MergedMap = MergedMap == null ? MakeTilemap("Merged Tilemap") : MergedMap;

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
            if (MergedMap == null) return;

            foreach (var pos in layer.cellBounds.allPositionsWithin)
            {
                if (!TryGetTile(pos, out var tile))
                    continue;

                var newPos = new Vector3Int(pos.x, pos.y, z);
                MergedMap.SetTile(newPos, tile);
            }
        }

        [ContextMenu("Split layers")]
        private void SplitLayers()
        {
            if (MergedMap == null) return;

            for (int i = MergedMap.cellBounds.zMin; i <= MergedMap.cellBounds.zMax; i++)
                SplitLayer(i);

            //this.SafeDestroy(MergedMap);
        }

        private void SplitLayer(int z = 0)
        {
            var tm = MakeTilemap("Split", z);
            var tiles = GetAllPossibleTiles(z);
            foreach (var tile in tiles)
                tm.SetTile(tile.Position, tile.Tile);

            Layers.Add(tm);
        }

        public Tilemap GetMap(int z)
        {
            if (MergedMap != null && Layers != null && Layers.Count > 0)
            {
                Debug.LogWarning("There can exist only one type of map! Preferring merged one instead.");
                return MergedMap;
            }

            if (MergedMap != null)
                return MergedMap;

            if (Layers.Count >= z)
                return Layers[z];
            else throw new System.ArgumentOutOfRangeException("layer");
            
            //return null;
        }

        private Tilemap MakeTilemap(string name, int z = 0)
        {
            var gameObject = new GameObject($"{name}_{z}");
            gameObject.transform.SetParent(this.transform, false);
            var tm = gameObject.AddComponent<Tilemap>();
            var renderer = gameObject.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = z - 1;
            return tm;
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(Bounds.position, Bounds.size);
        }

        public MapTileData GetTile(Vector3Int position)
        {
            var map = GetMap(position.z);
            var tile = map.GetTile(position);
            if (tile != null) return new MapTileData(map, position, tile);
            return null;
        }

        public bool TryGetTile(Vector3Int position, out MapTileData tile)
        {
            tile = GetTile(position);
            return tile != null;
        }

        public List<MapTileData> GetTiles(Vector2Int position)
        {
            var map = GetMap(0);
            var list = new List<MapTileData>();

            if (IsLayered)
            {
                foreach (var layer in Layers)
                {
                    var pos = new Vector3Int(position.x, position.y, Layers.IndexOf(layer));
                    var tile = GetTile(pos);
                    if (tile != null) list.Add(tile);
                }
            }
            else if (IsMerged)
            {
                for (int i = map.cellBounds.zMin; i <= map.cellBounds.zMax; i++)
                {
                    var pos = new Vector3Int(position.x, position.y, i);
                    var tile = GetTile(pos);
                    if (tile != null) list.Add(tile);
                }
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

            if (IsLayered)
            {
                Layers[position.z].SetTile(position, tile);
                Layers[position.z].RefreshTile(position);
            }
            else if (IsMerged)
            {
                MergedMap.SetTile(position, tile);
                MergedMap.RefreshTile(position);
            }
        }

        public List<MapTileData> GetTilesInRange(Vector2Int position, int range)
        {
            var list = new List<MapTileData>();

            if (IsLayered)
            {
                for (int i = 0; i < Layers.Count; i++)
                    list.AddRange(GetTilesInRange(new Vector3Int(position.x, position.y, i), range));
            }
            else if (IsMerged)
            {
                for (int y = position.y - range; y <= position.y + range; y++)
                    for (int x = position.x - range; x <= position.x + range; x++)
                        list.AddRange(GetTiles(new Vector2Int(x, y)));
            }

            list = list.Where(q => q != null).ToList();
            return list;
        }

        public List<MapTileData> GetAdjacentTiles(Vector2Int position, int range = 1)
            => GetTilesInRange(position, range)
            .Where(q => new Vector2Int(q.Position.x, q.Position.y) != position)
            .ToList();

        public List<MapTileData> GetTilesInRange(Vector3Int position, int range)
        {
            var list = new List<MapTileData>();
            for (int x = position.x - range; x <= position.x + range; x++)
                for (int y = position.y - range; y <= position.y + range; y++)
                    list.Add(GetTile(new Vector3Int(x, y, position.z)));

            list = list.Where(q => q != null).ToList();
            return list;
        }

        public List<MapTileData> GetAdjacentTiles(Vector3Int position, int range = 1)
            => GetTilesInRange(position, range)
            .Where(q => q.Position != position)
            .ToList();

        public List<MapTileData> GetAllPossibleTiles(int z = 0)
        {
            var map = GetMap(z);
            var list = new List<MapTileData>();
            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                if (pos.z != z) continue;
                var tile = map.GetTile(pos);
                if (tile != null) list.Add(new MapTileData(map, pos, tile));
            }
            return list;
        }

        #region Extensions

        public Vector3Int WorldToCell(Vector3 position)
            => Grid.WorldToCell(position);

        public Vector3 CellToWorld(Vector3Int position)
            => Grid.GetCellCenterWorld(position);

        public Vector2Int WorldToCell(Vector2 position)
            => (Vector2Int)Grid.WorldToCell(position);

        public Vector2 CellToWorld(Vector2Int position)
            => Grid.GetCellCenterWorld((Vector3Int)position);

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
