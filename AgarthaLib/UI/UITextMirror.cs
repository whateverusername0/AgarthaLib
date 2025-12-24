using TMPro;
using UnityEngine;

namespace Agartha.UI
{
    public class UITextMirror : MonoBehaviour
    {
        public TMP_Text Original, Copy;

        private void Update()
        {
            Copy.text = Original.text;
        }
    }
}