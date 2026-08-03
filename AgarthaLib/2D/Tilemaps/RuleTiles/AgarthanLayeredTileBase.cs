using System;

namespace AgarthaLib._2D.Tilemaps.RuleTiles
{
    public abstract class AgarthanLayeredTileBase<TLayer> : AgarthanTileBase where TLayer : Enum
    {
        protected TLayer _layer;

        public int Layer => (int)(object)_layer;
    }
}
