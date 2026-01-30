using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib.Grid
{
    [Serializable] public class TileData
    {
        public TilemapDefinition Tilemap;
        public Vector3Int Position;
        public TileBase Tile;

        public TileData(TilemapDefinition tilemap, Vector3Int position, TileBase tile)
        {
            Tilemap = tilemap;
            Position = position;
            Tile = tile;
        }
    }
}
