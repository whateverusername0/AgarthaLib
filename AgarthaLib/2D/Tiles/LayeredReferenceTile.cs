using System;
using UnityEngine;

namespace AgarthaLib._2D.Tiles
{
    /// <summary>
    ///     Reference tile with an enum inside.
    /// </summary>
    public abstract class LayeredReferenceTile<TLayer> : ReferenceTile where TLayer : Enum
    {
        [Header(nameof(LayeredReferenceTile<TLayer>))]
        [SerializeField] protected TLayer _layer;

        public int Layer => (int)(object)_layer;
    }
}
