using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Goodies.Audio
{
    public class AudioSystem : AgarthanSingleton<AudioSystem>
    {
        public void SetGlobalVolume(float volume)
            => AudioListener.volume = volume;

        public AudioSource PlayClip(AudioClip clip)
        {
            if (clip == null) return null;

            var go = new GameObject(clip.name);
            var auds = go.AddComponent<AudioSource>();
            auds.clip = clip;
            auds.Play();

            Destroy(go, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
            return auds;
        }
    }
}
