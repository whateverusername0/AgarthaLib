#if USING_TILEMAP_EXTRAS
using AgarthaLib._2D.Tiles;
using AgarthaLib.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tilemaps.RuleTiles
{
    [CreateAssetMenu(menuName = "AgarthaLib / Tiles / Blended rule tile")]
    public class BlendedRuleTile : RuleTile
    {
        [Header(nameof(BlendedRuleTile))]
        public bool EnableBlending = true;
        public List<string> BlendingTags = new();
        public ObjectWhitelist<string> TagWhitelist = new();

        public override bool RuleMatch(int neighbor, TileBase other)
        {
            switch (other)
            {
                case RuleOverrideTile rot:
                    other = rot.m_InstanceTile; break;
                case ReferenceTile atb:
                    other = atb.TileReference; break;
                default: break;
            }

            if (!EnableBlending)
                return base.RuleMatch(neighbor, other);

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
            => TagWhitelist.Pass(b.BlendingTags);
    }
}
#endif