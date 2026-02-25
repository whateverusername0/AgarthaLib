using UnityEngine;

namespace AgarthaLib.Grid.Tiles
{
    [CreateAssetMenu(menuName = "AgarthaLib / Grid / Tiles / Basic rule tile")]
    public class GridRuleTile : RuleTile, ICollisionProvider
    {
        public bool ProvidesCollisions;
        public bool IsProvidesCollisions() => ProvidesCollisions;
    }
}
