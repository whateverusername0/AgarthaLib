using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Agartha.Grid
{
    [Serializable] public class TileData
    {
        public Vector3Int Position;
        public TileBase Tile;

        public TileData(Vector3Int position, TileBase tile)
        {
            Position = position;
            Tile = tile;
        }
    }

    [Serializable] public class TileData<T> where T : TileBase
    {
        public Vector3Int Position;
        public T Tile;

        public TileData(Vector3Int position, T tile)
        {
            this.Position = position;
            this.Tile = tile;
        }
    }
}
