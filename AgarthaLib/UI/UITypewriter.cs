using System.Collections;
using TMPro;
using UnityEngine;

namespace AgarthaLib.UI
{
    public class UITypewriter : MonoBehaviour
    {
        public TMP_Text Text;
        [Multiline] public string TextToWrite;
        public int CPS = 10;
        public bool StartOnEnable = false;

        private void OnEnable()
        {
            Text = Text != null ? Text : GetComponent<TMP_Text>();

            if (StartOnEnable) Typewrite(TextToWrite);
        }

        public void Typewrite(string text)
        {
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