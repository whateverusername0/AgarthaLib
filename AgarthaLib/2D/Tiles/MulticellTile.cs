using AgarthaLib.Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tiles
{
    [CreateAssetMenu(menuName = "AgarthaLib / Tiles / Multicell tile")]
    public class MulticellTile : MulticellDataTile
    {
        public RectInt Shape;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);

            tileData.gameObject = _prefabReference;
        }

        public override bool StartUp(Vector3Int pos, ITilemap it, GameObject go)
        {
            var tilemap = it.GetTilemap();

            PrefabInstance = go;
            if (PrefabInstance == null)
                return false;

            if (PrefabInstance.TryGetComponent<SpriteRenderer>(out var sr)
            && it.GetTilemap().TryGetComponent<TilemapRenderer>(out var tr))
                sr.sortingOrder = tr.sortingOrder;

            foreach (var s in Shape.allPositionsWithin)
            {
                if (s == (Vector2Int)pos) continue;
                var child = CreateInstance<MulticellDataTile>();
                child.name = $"{name} (Data)";
                child.ParentPosition = pos;
                child.PrefabInstance = go;

                tilemap.SetTile(new Vector3Int(s.x, s.y, pos.z), child);
            }

            return true;
        }

        // removal of said tile should be handled in a separate script.
        // unity does not have a OnRemove() method we can override.
        // sucks.
    }
}
