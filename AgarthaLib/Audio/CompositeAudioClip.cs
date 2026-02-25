using AgarthaLib.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Audio
{
    [CreateAssetMenu(menuName = "AgarthaLib / Audio / Audio clip composite")]
    public class CompositeAudioClip : ScriptableObject
    {
        public List<AudioClip> Clips;

        public static implicit operator AudioClip(CompositeAudioClip pool)
            => pool.Clips.Combine();
    }
}
