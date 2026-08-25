using AgarthaLib.Attributes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tiles
{
    public class MulticellDataTile : TileBase
    {
        [EditorReadOnly] public Vector3Int? ParentPosition;
        [SerializeField] protected GameObject _prefabReference;
        [EditorReadOnly] public GameObject PrefabInstance;
    }
}
