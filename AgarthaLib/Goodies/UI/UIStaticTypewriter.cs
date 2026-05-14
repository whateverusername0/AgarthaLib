using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using System.Collections;
using TMPro;
using UnityEngine;

namespace AgarthaLib.Goodies.UI
{
    public class UIStaticTypewriter : AgarthanBehaviour
    {
        [ValidateNull] public TMP_Text Text;
        [Multiline] public string TextToWrite;
        public int CPS = 10;
        public bool StartOnEnable = false;

        private void OnEnable()
        {
            if (StartOnEnable)
                Typewrite(TextToWrite);
        }

        public void Typewrite(string text)
        {
            if (Text == null)
                return;

            StartCoroutine(DoTypewriter(text));
        }

        private IEnumerator DoTypewriter(string line)
        {
            string textBuffer = null;
            foreach (char c in line)
            {
                textBuffer += c;
                Text.text = textBuffer;
                yield return new WaitForSeconds(1 / CPS);
            }
        }
    }
}