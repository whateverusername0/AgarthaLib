using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Goodies.Audio
{
    public class AudioSourceController : AgarthanBehaviour
    {
        [ValidateNull] public AudioSource Source;
        public AudioMixerData Data;

        protected override void Update()
        {
            base.Update();

            if (Data == null) return;
            Source.volume = Data.Volume;
        }
    }
}
