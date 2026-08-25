using AgarthaLib.Attributes;
using AgarthaLib.Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tiles
{
    /// <summary>
    ///     Stores a reference to another tile and works accordingly
    ///     while overriding their prefab isntance.
    /// </summary>
    [CreateAssetMenu(menuName = "AgarthaLib / Tiles / Reference tile")]
    public class ReferenceTile : TileBase
    {
        [Header(nameof(ReferenceTile))]
        [SerializeField] private TileBase _tileReference;
        [SerializeField] private GameObject _prefabReferenceOverride;
        [EditorReadOnly] public GameObject PrefabInstance;

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

            if (_prefabReferenceOverride != null)
                td.gameObject = _prefabReferenceOverride;
        }

        public override void RefreshTile(Vector3Int pos, ITilemap it)
        {
            var rt = GetTileReference();
            if (rt != null) rt.RefreshTile(pos, it);
            else base.RefreshTile(pos, it);

            foreach (var neighbor in Extensions2D.NeighborPositions)
                it.RefreshTile(pos + neighbor);
        }

        public override bool StartUp(Vector3Int pos, ITilemap it, GameObject go)
        {
            PrefabInstance = go;
            if (PrefabInstance == null)
                return false;

            if (PrefabInstance.TryGetComponent<SpriteRenderer>(out var sr)
            && it.GetTilemap().TryGetComponent<TilemapRenderer>(out var tr))
                sr.sortingOrder = tr.sortingOrder;

            return true;
        }
    }
}