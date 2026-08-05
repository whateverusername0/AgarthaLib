#if USING_TILEMAP_EXTRAS
using AgarthaLib.Attributes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tilemaps.RuleTiles
{
    public abstract class AgarthanTileBase : TileBase
    {
        [SerializeField] private TileBase _tileReference;
        [ScriptableObjectIcon] virtual public TileBase TileReference
        {
            get => _tileReference;
            set => _tileReference = value;
        }

        public bool ProvidesCollision = false;

        [Header("Data")]
        public bool ShouldInstance = true;
        [EditorReadOnly] public bool Instanced = false;

        public virtual TileBase GetRuleTile()
            => TileReference;

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            if (!Application.isPlaying || !ShouldInstance || Instanced)
                return true;

            var inst = Instantiate(this);
            inst.Instanced = true;
            tilemap.GetComponent<Tilemap>().SetTile(position, inst);
            return false; // move onto instanced
        }

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