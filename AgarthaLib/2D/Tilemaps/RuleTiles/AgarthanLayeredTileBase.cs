using System;
using UnityEngine;

namespace AgarthaLib._2D.Tilemaps.RuleTiles
{
    public abstract class AgarthanLayeredTileBase<TLayer> : AgarthanTileBase where TLayer : Enum
    {
        [SerializeField] protected TLayer _layer;

        public int Layer => (int)(object)_layer;
    }
}
