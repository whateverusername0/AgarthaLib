using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Audio
{
    public class AudioSourceController : AgarthanBehaviour
    {
        [ValidateNull] public AudioSource Source;
        public AudioMixerData Data;

        private void Update()
        {
            if (Data == null) return;
            Source.volume = Data.Volume;
        }
    }
}
