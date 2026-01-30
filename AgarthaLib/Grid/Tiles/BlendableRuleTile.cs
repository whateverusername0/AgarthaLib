using AgarthaLib.Tags;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib.Grid.Tiles
{
    public class BlendableRuleTile : RuleTile, ITagsContainer
    {
        public List<string> Tags;
        public List<string> GetTags() => Tags;

        public TagWhitelist<string> Whitelist;

        public override bool RuleMatch(int neighbor, TileBase other)
        {
            if (other is RuleOverrideTile)
                other = (other as RuleOverrideTile).m_InstanceTile;

            switch (neighbor)
            {
                case TilingRuleOutput.Neighbor.This:
                    return other is ITagsContainer && Pass(other as ITagsContainer);
                case TilingRuleOutput.Neighbor.NotThis:
                    return other is not ITagsContainer || !Pass(other as ITagsContainer);
                default: return base.RuleMatch(neighbor, other);
            }
        }

        public bool Pass(ITagsContainer b) => Whitelist.Pass(b.GetTags());
    }
}
