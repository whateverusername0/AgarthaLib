using UnityEngine;
using UnityEngine.Audio;

namespace AgarthaLib.Audio
{
    [CreateAssetMenu(menuName = "AgarthaLib / Audio / Audio Mixer Data")]
    public class AudioMixerData : ScriptableObject
    {
        public AudioMixerGroup Group;
        [Range(0f, 1f)] public float Volume = 1f;
    }
}
