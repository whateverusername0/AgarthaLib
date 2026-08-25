using AgarthaLib._2D.Grids;
using AgarthaLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tilemaps.RuleTiles
{
    [CreateAssetMenu(menuName = "AgarthaLib / Tiles / Directional tile")]
    public class DirectionalTile : TileBase
    {
        [Header(nameof(DirectionalTile))]
        public Direction Back = Direction.Any;
        public Direction Front = Direction.South;
        public List<DirectionTileBaseRule> Rules = new();

        [SerializeField] private GameObject _prefabReferenceOverride;
        [EditorReadOnly] public GameObject PrefabInstance;

        public TileBase GetTile(Direction front, Direction back)
        {
            var match = Rules
                .Where(q => q.Matches(front, back))
                .Where(q => q.Tile != null)
                .FirstOrDefault();

            return match?.Tile;
        }

        public bool TryGetTile(Direction front, Direction back, out TileBase tile)
        {
            tile = GetTile(front, back);
            return tile != null;
        }

        public override bool GetTileAnimationData(Vector3Int pos, ITilemap it, ref TileAnimationData tad)
            => TryGetTile(Front, Back, out var tile) && tile.GetTileAnimationData(pos, it, ref tad);

        public override void GetTileData(Vector3Int pos, ITilemap it, ref TileData td)
        {
            if (TryGetTile(Front, Back, out var tile))
                tile.GetTileData(pos, it, ref td);

            if (_prefabReferenceOverride != null)
                td.gameObject = _prefabReferenceOverride;
        }

        public override void RefreshTile(Vector3Int pos, ITilemap it)
        {
            if (TryGetTile(Front, Back, out var tile))
                tile.RefreshTile(pos, it);
            base.RefreshTile(pos, it);
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            if (!TryGetTile(Front, Back, out var tile) || !tile.StartUp(position, tilemap, go))
                return false;

            PrefabInstance = go;
            return true;
        }
    }

    [Serializable] public class DirectionTileBaseRule
    {
        public Direction Back = Direction.Any;
        public Direction Front = Direction.North;
        public TileBase Tile;

        public bool Matches(Direction front, Direction back)
            => front == Front && back == Back;
    }
}
