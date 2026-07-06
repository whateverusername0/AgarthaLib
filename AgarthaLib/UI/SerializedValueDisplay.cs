#if USING_TMP
using AgarthaLib.Data.Serialization;
using AgarthaLib.Data.Serialization.SerializedTypes;
using AgarthaLib.MonoBehavior;
using System;
using TMPro;
using UnityEngine;

namespace AgarthaLib.UI
{
    [ExecuteInEditMode] public class SerializedValueDisplay<T, N> : AgarthanBehaviour
        where T : SerializedNumeric<N>
        where N : struct, IComparable<N>
    {
        public SerializedDictionary<T, TMP_Text> Links;

        protected override void Update()
        {
            base.Update();

            foreach (var item in Links)
            {
                if (item.Key == null || item.Value == null)
                    continue;

                item.Value.text = $"{item.Key.Value}{(!item.Key.Max.Equals(0) ? $" / {item.Key.Max}" : "")}";
            }
        }
    }
}
#endif