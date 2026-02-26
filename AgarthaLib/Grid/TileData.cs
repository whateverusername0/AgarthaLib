using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib.Grid
{
    [Serializable] public class TileData : TileData<TileBase>
    {
        public TileData(Tilemap tilemap, Vector3Int position) : base(tilemap, position) { }
        public TileData(Tilemap tilemap, Vector3Int position, TileBase tile) : base(tilemap, position, tile) { }
    }

    [Serializable] public class TileData<T> where T : TileBase
    {
        public Tilemap Tilemap;
        public Vector3Int Position;
        public T Tile;

        public TileData(Tilemap tilemap, Vector3Int position)
        {
            Position = position;
            Tile = null;
        }

        public TileData(Tilemap tilemap, Vector3Int position, T tile) : this(tilemap, position)
            => Tile = tile;

        public TileData(TileData<T> obj)
        {
            Tilemap = obj.Tilemap;
            Position = obj.Position;
            Tile = obj.Tile;
        }
    }
}