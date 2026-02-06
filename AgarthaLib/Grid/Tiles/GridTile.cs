using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib.Grid.Tiles
{
    [CreateAssetMenu(menuName = "AgarthaLib / Grid / Tiles / Basic tile")]
    public class GridTile : Tile, ICollisionProvider
    {
        public bool ProvidesCollisions;
        public bool IsProvidesCollisions() => ProvidesCollisions;
    }
}
