using AgarthaLib.Data.Serialization.SerializedTypes;
using AgarthaLib.MonoBehavior;
using System;
using UnityEngine;

namespace AgarthaLib._2D.Grids
{
    public abstract class LayerDataProvider<T> : AgarthanSingleton<LayerDataProvider<T>>
        where T : Enum
    {
        [SerializeField] private SerializedDictionary<T, LayerData> _data = new();

        public SerializedDictionary<T, LayerData> Data => _data;
    }
}
