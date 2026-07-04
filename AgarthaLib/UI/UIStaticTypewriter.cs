#if USING_TMP
using AgarthaLib.Attributes;
using AgarthaLib.Extensions;
using AgarthaLib.Goodies.Audio;
using AgarthaLib.MonoBehavior;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace AgarthaLib.UI
{
    public class UIStaticTypewriter : AgarthanBehaviour
    {
        private AudioSystem _aud => AudioSystem.Instance;

        [ValidateNull] public TMP_Text Text;
        [Multiline] public string TextToWrite;
        public int CPS = 10;
        public bool StartOnEnable = false;
        public AudioClip TypingSound;

        public UnityEvent OnFinishTypewriting;

        private void OnEnable()
        {
            if (StartOnEnable)
                Typewrite(TextToWrite);
        }

        public void Typewrite(string text)
        {
            if (Text == null)
                return;

            StartCoroutine(IETypewrite(text));
        }

        public IEnumerator IETypewrite(string text)
        {
            var textBuffer = string.Empty;
            foreach (char c in text)
            {
                textBuffer += c;
                Text.text = textBuffer;

                if (TypingSound != null)
                    _aud.PlayClip(TypingSound).WithPitch(Random.Range(1.25f, 1.5f)).WithVolume(.5f);

                yield return new WaitForSeconds(1f / CPS);
            }

            OnFinishTypewriting?.Invoke();
            yield return null;
        }
    }
}
#endif