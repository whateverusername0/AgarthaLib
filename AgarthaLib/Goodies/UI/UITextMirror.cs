using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using TMPro;

namespace AgarthaLib.Goodies.UI
{
    public class UITextMirror : AgarthanBehaviour
    {
        [ValidateNull] public TMP_Text Original;
        public TMP_Text Copy;

        protected override void Update()
        {
            Copy.text = Original.text;
        }
    }
}