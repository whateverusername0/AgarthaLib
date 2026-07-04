using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tilemaps
{
    [Serializable] public class MapTileData : MapTileData<TileBase>
    {
        public MapTileData(Tilemap tilemap, Vector3Int position) : base(tilemap, position) { }
        public MapTileData(Tilemap tilemap, Vector3Int position, TileBase tile) : base(tilemap, position, tile) { }
    }

    [Serializable] public class MapTileData<T> where T : TileBase
    {
        public Tilemap BoundTilemap;
        public Vector3Int Position;
        public T Tile;

        public MapTileData(Tilemap tilemap, Vector3Int position)
        {
            BoundTilemap = tilemap;
            Position = position;
            Tile = null;
        }

        public MapTileData(Tilemap tilemap, Vector3Int position, T tile) : this(tilemap, position)
            => Tile = tile;

        public MapTileData(MapTileData<T> obj)
        {
            BoundTilemap = obj.BoundTilemap;
            Position = obj.Position;
            Tile = obj.Tile;
        }

        public Vector3 GetWorldPosition()
            => Position + new Vector3(0.5f, 0.5f);

        public static implicit operator MapTileData(MapTileData<T> obj)
            => new(obj.BoundTilemap, obj.Position, obj.Tile);

        public static implicit operator TileBase(MapTileData<T> obj)
            => obj.Tile;

        // bloat
        public static bool operator ==(MapTileData<T> a, MapTileData<T> b)
        {
            if (a is null || a.Tile is null) return b is null;
            return b is not null && a.Position == b.Position;
        }

        public static bool operator !=(MapTileData<T> a, MapTileData<T> b)
            => !(a == b);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is null || obj.GetType() != GetType()) return false;

            var other = (MapTileData<T>)obj;
            return Position == other.Position;
        }

        public override int GetHashCode() => Position.GetHashCode();
    }
}
