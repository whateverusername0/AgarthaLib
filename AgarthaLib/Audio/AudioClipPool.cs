using AgarthaLib.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Audio
{
    [CreateAssetMenu(menuName = "Agartha / Audio / Audio clip pool")]
    public class AudioClipPool : ScriptableObject
    {
        public List<AudioClip> Clips;

        public static implicit operator AudioClip(AudioClipPool pool)
            => pool.Clips.PickRandom();
    }
}