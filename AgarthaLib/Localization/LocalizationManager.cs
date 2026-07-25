using AgarthaLib.Data;
using AgarthaLib.MonoBehavior;

namespace AgarthaLib.Localization
{
    public class LocalizationManager : AgarthanSingleton<LocalizationManager>, IDoNotDestroyOnLoad
    {
        public LocalizationAsset DefaultLocale;
        public LocalizationAsset CurrentLocale;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        /// <summary>
        ///     Changes the current localization asset and updates all strings to support it.
        /// </summary>
        public void SetLocale(LocalizationAsset locass)
        {
            CurrentLocale = locass;
            UpdateLocale();
        }

        /// <summary>
        ///     Gets the localized string, provided an ID
        ///     and if it exists in the localization files.
        /// </summary>
        public string GetLoc(string id)
        {
            if (CurrentLocale == null && DefaultLocale == null)
                return id;

            if (CurrentLocale != null && CurrentLocale.Contains(id))
                return CurrentLocale[id].Locale;

            else if (DefaultLocale != null && DefaultLocale.Contains(id))
                return DefaultLocale[id].Locale;

            return id;
        }

        /// <summary>
        ///     Gets the text of the current localization asset
        ///     instead of whatever exists in the LocID.
        /// </summary>
        public string GetLoc(LocId id)
            => GetLoc(id.ID); 

        protected void UpdateLocale()
        {
            foreach (var glue in FindObjectsOfType<LocalizationGlue>())
                glue.UpdateLocale();
        }
    }
}
