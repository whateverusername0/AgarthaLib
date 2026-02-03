using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib.Grid
{
    [Serializable] public class TileData : TileData<TileBase>
    {
        public TileData(TilemapDefinition tilemap, Vector3Int position, TileBase tile) : base(tilemap, position, tile) { }
    }

    [Serializable] public class TileData<T> where T : TileBase
    {
        public TilemapDefinition Tilemap;
        public Vector3Int Position;
        public T Tile;

        public TileData(TilemapDefinition tilemap, Vector3Int position, T tile)
        {
            Tilemap = tilemap;
            Position = position;
            Tile = tile;
        }

        public TileData(TileData<T> obj)
        {
            Tilemap = obj.Tilemap;
            Position = obj.Position;
            Tile = obj.Tile;
        }

        public static implicit operator TileData(TileData<T> @this)
            => @this;
    }
}
