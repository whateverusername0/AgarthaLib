using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Audio
{
    public class AudioSystem : AgarthanSingleton<AudioSystem>
    {
        public List<AudioMixerEntry> Groups;

        public AudioSource PlayPvs(AudioClip clip, Vector3 position, AudioMixerGroupEnum group)
        {
            var go = new GameObject(clip.name);
            go.transform.position = position;

            var groupEntry = Groups.Where(q => q.Type == group).FirstOrDefault();
            var auds = go.AddComponent<AudioSource>();
            auds.clip = clip;
            auds.outputAudioMixerGroup = groupEntry != null ? groupEntry.Data.Group : null;
            auds.volume = groupEntry != null ? groupEntry.Data.Volume : 1f;
            auds.Play();

            Destroy(go, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
            return auds;
        }
    }

    [Serializable] public class AudioMixerEntry
    {
        public AudioMixerGroupEnum Type;
        public AudioMixerData Data;
    }

    [Serializable] public enum AudioMixerGroupEnum
    {
        Master,
        Music,
        Sound
    }
}
