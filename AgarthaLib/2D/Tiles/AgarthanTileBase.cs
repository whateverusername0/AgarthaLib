using AgarthaLib.Attributes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tilemaps.RuleTiles
{
    public abstract class AgarthanTileBase : TileBase
    {
        [Header(nameof(AgarthanTileBase))]
        [SerializeField] private TileBase _tileReference;
        [ScriptableObjectIcon] virtual public TileBase TileReference
        {
            get => _tileReference;
            set => _tileReference = value;
        }

        public virtual TileBase GetTileReference()
            => TileReference;

        public override bool GetTileAnimationData(Vector3Int pos, ITilemap it, ref TileAnimationData tad)
        {
            var rt = GetTileReference();
            return rt != null && rt.GetTileAnimationData(pos, it, ref tad);
        }

        public override void GetTileData(Vector3Int pos, ITilemap it, ref TileData td)
        {
            var rt = GetTileReference();
            if (rt != null) rt.GetTileData(pos, it, ref td);
            else base.GetTileData(pos, it, ref td);
        }

        private readonly Vector3Int[] _neighborPositions = new Vector3Int[]
        {
            new(-1, 1, 0),  new(0, 1, 0),  new(1, 1, 0),
            new(-1, 0, 0), new(1, 0, 0),
            new(-1, -1, 0), new(0, -1, 0), new(1, -1, 0)
        };

        public override void RefreshTile(Vector3Int pos, ITilemap it)
        {
            var rt = GetTileReference();
            if (rt != null) rt.RefreshTile(pos, it);
            else base.RefreshTile(pos, it);

            foreach (var neighbor in _neighborPositions)
                it.RefreshTile(pos + neighbor);
        }
    }
}