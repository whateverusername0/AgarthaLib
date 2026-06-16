using AgarthaLib.MonoBehavior;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Localization
{
    public class LocalizationGlue : AgarthanBehaviour
    {
        private LocalizationManager _loc => LocalizationManager.Instance;

        public string LocID;

        protected override void Start()
        {
            base.Start();
            UpdateLocale();
        }

        public void UpdateLocale()
        {
            var locale = _loc.GetLoc(LocID);
            foreach (var comp in GetComponents<Component>())
                ResolveText(comp, locale);
        }

        protected void ResolveText(Component comp, string locText)
        {
            switch (comp)
            {
                case Text text: text.text = locText; break;
                case TMP_Text tmptext: tmptext.text = locText; break;
                // todo add shit here
                default: break;
            }
        }
    }
}