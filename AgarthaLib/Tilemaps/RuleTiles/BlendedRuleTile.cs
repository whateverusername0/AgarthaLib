using AgarthaLib.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib.Tilemaps.RuleTiles
{
    [CreateAssetMenu(menuName = "AgarthaLib / Grid / Tiles / Blended rule tile")]
    public class BlendedRuleTile : RuleTile
    {
        public List<string> Tags;

        public ObjectWhitelist<string> Whitelist;

        public override bool RuleMatch(int neighbor, TileBase other)
        {
            if (other is RuleOverrideTile)
                other = (other as RuleOverrideTile).m_InstanceTile;

            switch (neighbor)
            {
                case TilingRuleOutput.Neighbor.This:
                    return other is BlendedRuleTile && Pass(other as BlendedRuleTile);
                case TilingRuleOutput.Neighbor.NotThis:
                    return other is not BlendedRuleTile || !Pass(other as BlendedRuleTile);
                default: return base.RuleMatch(neighbor, other);
            }
        }

        public bool Pass(BlendedRuleTile b)
            => Whitelist.Pass(b.Tags);
    }
}
