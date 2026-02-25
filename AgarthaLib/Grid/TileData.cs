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
            Tilemap = tilemap;
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

        public Vector3 GetWorldPosition()
            => Position + new Vector3(0.5f, 0.5f);

        public static implicit operator TileData(TileData<T> obj)
            => new(obj.Tilemap, obj.Position, obj.Tile);

        public static implicit operator TileBase(TileData<T> obj)
            => obj.Tile;

        // bloat
        public static bool operator ==(TileData<T> a, TileData<T> b)
        {
            if (a is null || a.Tile is null) return b is null;
            return b is not null && a.Position == b.Position;
        }

        public static bool operator !=(TileData<T> a, TileData<T> b)
            => !(a == b);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is null || obj.GetType() != GetType()) return false;

            var other = (TileData<T>)obj;
            return Position == other.Position;
        }

        public override int GetHashCode() => Position.GetHashCode();
    }
}
