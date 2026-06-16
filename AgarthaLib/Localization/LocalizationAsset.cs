using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Localization
{
    [CreateAssetMenu(menuName = "AgarthaLib / Localization / Localization Asset")]
    [Serializable] public class LocalizationAsset : ScriptableObject
    {
        public string LocalizedName;
        public List<LocId> Locales = new();

        #region Operators

        public bool Contains(string locid)
            => Locales.Contains(locid);

        // may cause crashes idk jic
        public LocId this[string index]
            => Locales[Locales.IndexOf(index)];

        public static implicit operator List<LocId>(LocalizationAsset asset)
            => asset.Locales;

        public static implicit operator Dictionary<string, string>(LocalizationAsset asset)
        {
            var dict = new Dictionary<string, string>();
            foreach (var entry in asset.Locales)
                dict.Add(entry.ID, entry.Locale);
            return dict;
        }

        #endregion
    }
}
