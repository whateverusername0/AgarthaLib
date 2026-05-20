using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using TMPro;
using UnityEngine;

namespace AgarthaLib.Goodies.UI
{
    [ExecuteInEditMode] public class TextClone : AgarthanBehaviour
    {
        [ValidateNull] public TMP_Text Original;
        public TMP_Text Copy;

        protected override void Update()
        {
            Copy.text = Original.text;
            Copy.alpha = Original.alpha;
        }
    }
}