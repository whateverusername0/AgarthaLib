using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Agartha.Grid
{
    public class TilemapData : MonoBehaviour
    {
        public Tilemap Map;

        public List<TileData> GetTiles() => GetTiles(Map);

        public List<TileData> GetTiles(Tilemap map, bool compress = true)
        {
            var @out = new List<TileData>();

            if (compress) map.CompressBounds();

            var bounds = map.cellBounds;
            var allTiles = map.GetTilesBlock(bounds);

            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                if (map.HasTile(localPlace))
                    @out.Add(new(localPlace, map.GetTile(localPlace)));
            }

            return @out;
        }

        public List<TileData> GetAdjacentTiles(TileData tile)
        {
            var @out = new List<TileData>();
            var directions = new Vector3Int[]
            {
                new(1, 0), new(-1, 0),
                new(0, 1), new(0, -1),
                new(1, 1), new(1, -1),
                new(-1, 1), new(-1, -1),
            };

            var tiles = GetTiles();
            foreach (var dir in directions)
            {
                var neighborPos = tile.Position + dir;
                @out.Add(tiles.FirstOrDefault(q => q.Position == neighborPos));
            }

            return @out;
        }

        public Vector3 GetWorldPosition(TileData tile)
            => Map.CellToWorld(tile.Position);

        public Vector3 GetWorldPosition<T>(TileData<T> tile) where T : TileBase
            => Map.GetCellCenterWorld(tile.Position);

        public TileData GetTile(Vector3 worldPosition)
            => GetTiles().FirstOrDefault(q => Map.GetCellCenterWorld(q.Position) == worldPosition);
    }
}