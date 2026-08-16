using AgarthaLib.Data.Fields;
using System;
using UnityEngine;

namespace AgarthaLib._2D.Grids
{
    [Serializable] public class LayerData : IEquatable<LayerData>
    {
        public bool ProvidesCollision = false;
        public bool IsTrigger = false;

        public bool ShouldRender = true;
        public Material RenderMaterial = null;
        public SortingLayerField SortingLayer = default;

        public LayerMask CollisionLayer = 0;

        #region Boilerplate

        public bool Equals(LayerData other)
            => other != null && CollisionLayer == other.CollisionLayer;

        public override bool Equals(object obj)
            => obj is LayerData && Equals(obj as LayerData);

        public override int GetHashCode()
            => base.GetHashCode();

        public static bool operator ==(LayerData a, LayerData b)
            => a?.CollisionLayer == b?.CollisionLayer;

        public static bool operator !=(LayerData a, LayerData b)
            => !(a == b);

        #endregion
    }
}
