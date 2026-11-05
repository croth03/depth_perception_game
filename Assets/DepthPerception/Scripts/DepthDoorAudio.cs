using System;
using UnityEngine;

namespace DepthPerception
{
    public sealed class DepthDoorAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource ambienceSource;
        [SerializeField] private AudioSource crossingSource;
        [SerializeField] private AudioClip[] ambienceClips = Array.Empty<AudioClip>();
        [SerializeField] private AudioClip[] crossingClips = Array.Empty<AudioClip>();
        [SerializeField, Range(0f, 1f)] private float ambienceVolume = 0.24f;
        [SerializeField, Range(0f, 1f)] private float crossingVolume = 0.75f;

        private int lastAmbience = -1;
        private int lastCrossing = -1;
        private bool crossed;

        private void Awake()
        {
            if (ambienceSource == null)
                ambienceSource = CreateSource("Door Ambience", true, ambienceVolume);
            if (crossingSource == null)
                crossingSource = CreateSource("Door Crossing", true, crossingVolume);
            AudioClip ambience = Choose(ambienceClips, ref lastAmbience);
            if (ambience != null)
            {
                ambienceSource.clip = ambience;
                ambienceSource.loop = true;
                ambienceSource.Play();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (crossed || other.GetComponentInParent<DepthPlayerController>() == null)
                return;
            crossed = true;
            AudioClip crossing = Choose(crossingClips, ref lastCrossing);
            if (crossing != null)
                crossingSource.PlayOneShot(crossing, crossingVolume);
        }

        private AudioSource CreateSource(string sourceName, bool spatial, float volume)
        {
            GameObject host = new GameObject(sourceName);
            host.transform.SetParent(transform, false);
            AudioSource source = host.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = spatial ? 1f : 0f;
            source.volume = volume;
            source.minDistance = 3f;
            source.maxDistance = 30f;
            return source;
        }

        private static AudioClip Choose(AudioClip[] clips, ref int previous)
        {
            if (clips == null || clips.Length == 0)
                return null;
            int index = clips.Length == 1 ? 0 : UnityEngine.Random.Range(0, clips.Length);
            if (clips.Length > 1 && index == previous)
                index = (index + 1 + UnityEngine.Random.Range(0, clips.Length - 1)) % clips.Length;
            previous = index;
            return clips[index];
        }
    }
}
