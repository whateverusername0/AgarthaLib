using AgarthaLib.Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tiles
{
    [CreateAssetMenu(menuName = "AgarthaLib / Tiles / Multicell tile")]
    public class MulticellTile : MulticellDataTile
    {
        public RectInt Shape;

        public override bool StartUp(Vector3Int pos, ITilemap it, GameObject go)
        {
            var tilemap = it.GetTilemap();

            var prefabRef = PrefabReference != null
                ? Instantiate(PrefabReference, tilemap.transform)
                : null;

            foreach (var s in Shape.allPositionsWithin)
            {
                if (s == (Vector2Int)pos) continue;
                var child = CreateInstance<MulticellDataTile>();
                child.name = $"{name} (Data)";
                child.ParentPosition = pos;
                child.PrefabReference = prefabRef;

                tilemap.SetTile(new Vector3Int(s.x, s.y, pos.z), child);
            }

            return true;
        }

        // removal of said tile should be handled in a separate script.
        // unity does not have a OnRemove() method we can override.
        // sucks.
    }
}
