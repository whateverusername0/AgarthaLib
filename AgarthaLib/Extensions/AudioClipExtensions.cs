using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Extensions
{
    public static class AudioClipExtensions
    {
        public static AudioClip Combine(this List<AudioClip> clips)
        {
            if (clips == null || clips.Count == 0)
                return null;

            var sampleRate = clips[0].frequency;
            var channels = clips[0].channels;
            var combined = new List<float>();

            foreach (var c in clips)
            {
                if (c.frequency != sampleRate || c.channels != channels)
                {
                    Debug.LogError("All AudioClips must have the same sample rate and number of channels to be combined.");
                    return null;
                }

                float[] data = new float[c.samples * c.channels];
                c.GetData(data, 0);
                combined.AddRange(data);
            }

            var newclip = AudioClip.Create("CombinedClip", combined.Count / channels, channels, sampleRate, false);
            newclip.SetData(combined.ToArray(), 0);

            return newclip;
        }
    }
}
