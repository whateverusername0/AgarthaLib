using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib.Grid
{
    public class TilemapDefinition : AgarthanBehaviour
    {
        public MapDefinition MapDefinition;
        public Tilemap Tilemap;
        public List<TileData> Tiles;
        public bool ProvideCollisions = false;

        private void Start()
        {
            Tiles = GetTiles();
        }

        public List<TileData> GetTiles(bool compress = true)
        {
            var @out = new List<TileData>();

            if (compress) Tilemap.CompressBounds();

            var bounds = Tilemap.cellBounds;
            foreach (var pos in bounds.allPositionsWithin)
            {
                var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                if (Tilemap.HasTile(localPlace))
                    @out.Add(new(this, localPlace, Tilemap.GetTile(localPlace)));
            }

            return @out;
        }

        public TileData GetTile(Vector3 pos)
        {
            var local = Tiles.Where(q => q.Position == pos).FirstOrDefault();
            if (local != null) return local;

            var world = Tiles.FirstOrDefault(q => Tilemap.GetCellCenterWorld(q.Position) == pos);
            if (world != null) return world;

            return null;
        }

        public TileData SetTile(Vector3Int position, TileBase tile)
        {
            Tilemap.SetTile(position, tile);
            var data = new TileData(this, position, tile);

            var existing = GetTile(position);
            if (existing != null) Tiles[Tiles.IndexOf(existing)] = data;
            else Tiles.Add(data);

            return data;
        }

        public List<TileData> GetAdjacentTiles(TileData tile, bool diagonal = true)
        {
            var @out = new List<TileData>();
            var directions = new List<Vector3Int>
            {
                new(1, 0), new(-1, 0),
                new(0, 1), new(0, -1)
            };

            var diagonalDirections = new List<Vector3Int>
            {
                new(1, 1), new(1, -1),
                new(-1, 1), new(-1, -1)
            };

            if (diagonal) directions.AddRange(diagonalDirections);

            foreach (var dir in directions)
            {
                var neighborPos = tile.Position + dir;
                @out.Add(GetTile(neighborPos));
            }

            return @out;
        }

        public Vector3 GetWorldPosition(TileData tile)
            => Tilemap.CellToWorld(tile.Position);
    }
}