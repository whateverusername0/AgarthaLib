using AgarthaLib.Attributes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tiles
{
    public class MulticellDataTile : TileBase
    {
        [EditorReadOnly] public Vector3Int? ParentPosition;
        public GameObject PrefabReference;
    }
}
