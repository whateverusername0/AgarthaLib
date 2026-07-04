using AgarthaLib.MonoBehavior;

namespace AgarthaLib.Localization
{
    public abstract class LocalizationGlue : AgarthanBehaviour
    {
        private LocalizationManager _loc => LocalizationManager.Instance;

        public string LocID;

        protected override void Start()
        {
            base.Start();
            UpdateLocale();
        }

        public virtual void UpdateLocale()
        {
            var locale = _loc.GetLoc(LocID);
            SetText(locale);
        }

        protected abstract void SetText(string locale);
    }
}