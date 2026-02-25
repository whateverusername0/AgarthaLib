using AgarthaLib.Data;
using AgarthaLib.Grid.Tiles;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib.Grid
{
    /// <summary>
    ///     Combines and represents multiple tilemaps as a single map. Also provides helper methods.
    /// </summary>
    /// <remarks>
    ///     Multiple tilemaps are considered multiple Z layers that are combined on Start.
    /// </remarks>
    public class MapDefinition : AgarthanBehaviour
    {
        [Header("Tilemap")]
        public List<Tilemap> Layers;
        public Tilemap MasterTilemap;
        public string MapName = "Master Tilemap";

        [Header("Bounds")]
        public Box3D Bounds;
        public bool DefineBoundsOnStart = false;

        protected override void Start()
        {
            base.Start();

            if (MasterTilemap == null)
            {
                var mt = new GameObject(MapName);
                mt.transform.SetParent(this.transform, false);
                MasterTilemap = mt.AddComponent<Tilemap>();
                var mtr = mt.AddComponent<TilemapRenderer>();
                mtr.sortingOrder = -1;
            }

            MasterTilemap.name = MapName;

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

        protected void OnDrawGizmos()
        {
            // draws a cube.
            Gizmos.DrawLine(new(Bounds.X.Min, Bounds.Y.Min, Bounds.Z.Min), new(Bounds.X.Max, Bounds.Y.Min, Bounds.Z.Min));
            Gizmos.DrawLine(new(Bounds.X.Min, Bounds.Y.Min, Bounds.Z.Min), new(Bounds.X.Min, Bounds.Y.Max, Bounds.Z.Min));
            Gizmos.DrawLine(new(Bounds.X.Max, Bounds.Y.Max, Bounds.Z.Max), new(Bounds.X.Min, Bounds.Y.Max, Bounds.Z.Max));
            Gizmos.DrawLine(new(Bounds.X.Max, Bounds.Y.Max, Bounds.Z.Max), new(Bounds.X.Max, Bounds.Y.Min, Bounds.Z.Max));

            //Gizmos.DrawLine(new(Bounds.X.Min, Bounds.Y.Min, Bounds.Z.Min), new(Bounds.X.Max, Bounds.Y.Max, Bounds.Z.Max));
            //Gizmos.DrawLine(new(Bounds.X.Min, Bounds.Y.Min, Bounds.Z.Min), new(Bounds.X.Max, Bounds.Y.Max, Bounds.Z.Max));
            //Gizmos.DrawLine(new(Bounds.X.Min, Bounds.Y.Min, Bounds.Z.Min), new(Bounds.X.Max, Bounds.Y.Max, Bounds.Z.Max));
            //Gizmos.DrawLine(new(Bounds.X.Min, Bounds.Y.Min, Bounds.Z.Min), new(Bounds.X.Max, Bounds.Y.Max, Bounds.Z.Max));

            //Gizmos.DrawLine(new(Bounds.X.Min, Bounds.Y.Min, Bounds.Z.Min), new(Bounds.X.Max, Bounds.Y.Max, Bounds.Z.Max));
            //Gizmos.DrawLine(new(Bounds.X.Min, Bounds.Y.Min, Bounds.Z.Min), new(Bounds.X.Max, Bounds.Y.Max, Bounds.Z.Max));
            //Gizmos.DrawLine(new(Bounds.X.Min, Bounds.Y.Min, Bounds.Z.Min), new(Bounds.X.Max, Bounds.Y.Max, Bounds.Z.Max));
            //Gizmos.DrawLine(new(Bounds.X.Min, Bounds.Y.Min, Bounds.Z.Min), new(Bounds.X.Max, Bounds.Y.Max, Bounds.Z.Max));
        }

        private void MergeLayer(Tilemap layer, int z = 0)
        {
            if (MasterTilemap == null)
                return;

            foreach (var pos in layer.cellBounds.allPositionsWithin)
            {
                var tile = layer.GetTile(pos);
                if (tile != null)
                {
                    var newPos = new Vector3Int(pos.x, pos.y, z);
                    MasterTilemap.SetTile(newPos, tile);
                }
            }
        }

        public TileData GetTile(Vector3Int position)
        {
            var tile = MasterTilemap.GetTile(position);
            if (tile != null) return new TileData(MasterTilemap, position, tile);
            else return null;
        }

        public Vector3Int GetCellAt(Vector3 position)
            => MasterTilemap.WorldToCell(position);

        public List<TileData> GetTiles(Vector2Int position)
        {
            var list = new List<TileData>();
            foreach (var pos in Bounds.ToArray())
            {
                var tile = GetTile(new Vector3Int(pos.x, pos.y, pos.z));
                if (tile != null) list.Add(tile);
            }
            return list;
        }

        // picks the highest in order.
        public TileData GetTile(Vector2Int position)
        {
            var tiles = GetTiles(position).ToList();
            // return new empty data but with a null tile.
            if (tiles.Count == 0) return new(MasterTilemap, new(position.x, position.y, 0), null);
            else return tiles.OrderByDescending(q => q.Position.z).First();
        }

        public void SetTile(Vector3Int position, TileBase tile)
        {
            if (!Bounds.IsInBounds(position))
            {
                Debug.LogError($"Tile with position of {position} is out of bounds {Bounds}.");
                return;
            }

            // in favor of scriptable objects
            var existing = GetTile(position);
            if (existing != null) Destroy(existing.Tile);
            if (tile != null) tile = Instantiate(tile);

            MasterTilemap.SetTile(position, tile);
            MasterTilemap.RefreshTile(position);
        }

        public List<TileData> GetTilesInRange(Vector2Int position, int range)
        {
            var list = new List<TileData>();
            for (int x = position.x - range; x <= position.x + range; x++)
                for (int y = position.y - range; y <= position.y + range; y++)
                    list.Add(GetTile(new Vector2Int(x, y)));
            return list;
        }

        public List<TileData> GetAdjacentTiles(Vector2Int position)
            => GetTilesInRange(position, 1)
            .Where(q => new Vector2Int(q.Position.x, q.Position.y) != position)
            .ToList();

        public List<TileData> GetAdjacentTiles(Vector3Int position)
            => GetAdjacentTiles(new Vector2Int(position.x, position.y));

        public List<TileData> GetAllPossibleTiles()
        {
            var list = new List<TileData>();
            foreach (var pos in MasterTilemap.cellBounds.allPositionsWithin)
            {
                var tile = MasterTilemap.GetTile(pos);
                if (tile != null) list.Add(new TileData(MasterTilemap, pos, tile));
            }
            return list;
        }

        public bool IsWalkable(TileData data)
        {
            if (data != null && data.Tile is ICollisionProvider { } gt)
                return !gt.IsProvidesCollisions();
            else return true;
        }

        public bool IsWalkable(Vector2Int position)
            => IsWalkable(GetTile(position));
    }
}