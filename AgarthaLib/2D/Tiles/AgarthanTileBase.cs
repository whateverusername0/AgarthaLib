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

        public override void GetTileData(Vector3Int position, ITilemap it, ref TileData td)
        {
            var rt = GetTileReference();
            if (rt != null) rt.GetTileData(position, it, ref td);
            else base.GetTileData(position, it, ref td);
        }

        public override void RefreshTile(Vector3Int position, ITilemap it)
        {
            var rt = GetTileReference();
            if (rt != null) rt.RefreshTile(position, it);
            else base.RefreshTile(position, it);
        }
    }
}