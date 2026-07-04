#if USING_TILEMAP_EXTRAS
using AgarthaLib._2D.Tilemaps.Interfaces;
using AgarthaLib.Attributes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tilemaps.RuleTiles
{
    public abstract class AgarthanTileBase : TileBase, ICollisionProvider
    {
        [SerializeField] private RuleTile _ruleTileReference;
        [ScriptableObjectIcon] virtual public RuleTile RuleTileReference
        {
            get => _ruleTileReference;
            set => _ruleTileReference = value;
        }

        public bool ProvidesCollision = false;

        bool ICollisionProvider.ProvidesCollision()
            => ProvidesCollision;

        public virtual RuleTile GetRuleTile()
            => RuleTileReference;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            var rt = GetRuleTile();
            if (rt != null) rt.GetTileData(position, tilemap, ref tileData);
            else base.GetTileData(position, tilemap, ref tileData);
        }

        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            var rt = GetRuleTile();
            if (rt != null) rt.RefreshTile(position, tilemap);
            else base.RefreshTile(position, tilemap);
        }
    }
}
#endif