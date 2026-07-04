using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AgarthaLib.Extensions
{
    public static class AudioExtensions
    {
        public static AudioClip Combine(this List<AudioClip> clips)
        {
            if (clips == null || clips.Count == 0)
                return null;

            var sampleRate = clips[0].frequency;
            var channels = clips[0].channels;
            var combined = new List<float>();
            var namesb = new StringBuilder();

            foreach (var c in clips)
            {
                if (c.frequency != sampleRate || c.channels != channels)
                {
                    Debug.LogError("All clips must have the same sample rate and number of channels to be combined.");
                    return null;
                }

                float[] data = new float[c.samples * c.channels];
                c.GetData(data, 0);
                combined.AddRange(data);
                namesb.Append($"+{c.name}");
            }

            var newclip = AudioClip.Create(namesb.ToString(), combined.Count / channels, channels, sampleRate, false);
            newclip.SetData(combined.ToArray(), 0);

            return newclip;
        }

        public static AudioSource WithPitch(this AudioSource ass, float pitch)
        {
            if (ass == null) return null;

            ass.pitch = pitch;
            return ass;
        }

        public static AudioSource WithVolume(this AudioSource ass, float volume)
        {
            if (ass == null) return null;

            ass.volume = volume;
            return ass;
        }

        public static AudioSource WithRange(this AudioSource ass, float minDistance, float maxDistance)
        {
            if (ass == null) return null;

            ass.minDistance = minDistance;
            ass.maxDistance = maxDistance;
            ass.spatialBlend = 1f;
            return ass;
        }

        public static AudioSource Pvs(this AudioSource ass, Vector3 position)
        {
            if (ass == null) return null;

            var go = ass.gameObject;
            go.transform.position = position;
            ass.spatialBlend = 1f;
            return ass;
        }
    }
}
