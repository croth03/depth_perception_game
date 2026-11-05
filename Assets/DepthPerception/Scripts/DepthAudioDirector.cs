using System;
using System.Collections;
using UnityEngine;

namespace DepthPerception
{
    public sealed class DepthAudioDirector : MonoBehaviour
    {
        [Header("State source")]
        [SerializeField] private DepthCameraStateController cameraState;

        [Header("Perception music")]
        [SerializeField] private AudioClip[] realityMusic = Array.Empty<AudioClip>();
        [SerializeField] private AudioClip[] hallucinationMusic = Array.Empty<AudioClip>();
        [SerializeField] private AudioClip[] lostRealityMusic = Array.Empty<AudioClip>();
        [SerializeField, Range(0f, 1f)] private float musicVolume = 0.42f;
        [SerializeField, Min(0.1f)] private float musicCrossfadeDuration = 3.5f;

        [Header("Continuous valley ambience")]
        [SerializeField] private AudioClip[] valleyWind = Array.Empty<AudioClip>();
        [SerializeField, Range(0f, 1f)] private float ambienceVolume = 0.34f;

        [Header("Shared one-shots")]
        [SerializeField] private AudioClip[] transitionSounds = Array.Empty<AudioClip>();
        [SerializeField] private AudioClip[] doorCrossingSounds = Array.Empty<AudioClip>();

        [Header("Voice ducking")]
        [SerializeField, Range(0.1f, 1f)] private float dialogueDuck = 0.48f;
        [SerializeField, Range(0f, 1f)] private float voiceVolume = 0.95f;
        [SerializeField, Min(0.05f)] private float duckSpeed = 2.5f;

        [Header("Sources")]
        [SerializeField] private AudioSource musicA;
        [SerializeField] private AudioSource musicB;
        [SerializeField] private AudioSource ambienceSource;
        [SerializeField] private AudioSource voiceSource;
        [SerializeField] private AudioSource oneShotSource;

        private PerceptionState activeState = (PerceptionState)(-1);
        private AudioSource incomingMusic;
        private AudioSource outgoingMusic;
        private float crossfade;
        private float currentDuck = 1f;
        private int lastReality = -1;
        private int lastHallucination = -1;
        private int lastLostReality = -1;
        private int lastWind = -1;
        private int lastTransition = -1;
        private int lastDoorCrossing = -1;

        public bool IsVoicePlaying => voiceSource != null && voiceSource.isPlaying;

        private void Awake()
        {
            EnsureSources();
            if (cameraState == null)
                cameraState = FindAnyObjectByType<DepthCameraStateController>();
            StartWind();
            SetState(cameraState != null ? cameraState.CurrentState : PerceptionState.Reality, true);
        }

        private void Update()
        {
            PerceptionState state = cameraState != null ? cameraState.CurrentState : PerceptionState.Reality;
            if (state != activeState)
                SetState(state, false);

            float targetDuck = IsVoicePlaying ? dialogueDuck : 1f;
            currentDuck = Mathf.MoveTowards(currentDuck, targetDuck, duckSpeed * Time.unscaledDeltaTime);

            if (incomingMusic != null && outgoingMusic != null && crossfade < 1f)
            {
                crossfade = Mathf.MoveTowards(crossfade, 1f, Time.unscaledDeltaTime / Mathf.Max(0.1f, musicCrossfadeDuration));
                incomingMusic.volume = Mathf.SmoothStep(0f, musicVolume, crossfade) * currentDuck;
                outgoingMusic.volume = Mathf.SmoothStep(musicVolume, 0f, crossfade) * currentDuck;
                if (crossfade >= 1f)
                    outgoingMusic.Stop();
            }
            else if (incomingMusic != null)
            {
                incomingMusic.volume = musicVolume * currentDuck;
            }

            if (ambienceSource != null)
                ambienceSource.volume = ambienceVolume * currentDuck;
        }

        public IEnumerator PlayVoiceAndWait(AudioClip clip, float postDelay = 0.15f)
        {
            EnsureSources();
            if (clip == null || voiceSource == null)
            {
                if (postDelay > 0f)
                    yield return new WaitForSecondsRealtime(postDelay);
                yield break;
            }

            voiceSource.Stop();
            voiceSource.clip = clip;
            voiceSource.volume = voiceVolume;
            voiceSource.Play();
            while (voiceSource != null && voiceSource.isPlaying)
                yield return null;
            if (postDelay > 0f)
                yield return new WaitForSecondsRealtime(postDelay);
        }

        public void PlayTransition() => PlayRandomOneShot(transitionSounds, ref lastTransition, 0.7f);
        public void PlayDoorCrossing() => PlayRandomOneShot(doorCrossingSounds, ref lastDoorCrossing, 0.85f);

        public void PlayRandomOneShot(AudioClip[] clips, ref int previousIndex, float volume = 1f)
        {
            EnsureSources();
            AudioClip clip = Choose(clips, ref previousIndex);
            if (clip != null && oneShotSource != null)
                oneShotSource.PlayOneShot(clip, Mathf.Clamp01(volume));
        }

        private void SetState(PerceptionState state, bool immediate)
        {
            activeState = state;
            AudioClip next = state == PerceptionState.Reality
                ? Choose(realityMusic, ref lastReality)
                : state == PerceptionState.Hallucination
                    ? Choose(hallucinationMusic, ref lastHallucination)
                    : Choose(lostRealityMusic, ref lastLostReality);
            if (next == null)
                return;

            AudioSource current = incomingMusic != null && incomingMusic.isPlaying ? incomingMusic : musicA;
            AudioSource alternate = current == musicA ? musicB : musicA;
            if (current != null && current.clip == next && current.isPlaying)
                return;

            outgoingMusic = current;
            incomingMusic = alternate;
            incomingMusic.Stop();
            incomingMusic.clip = next;
            incomingMusic.loop = true;
            incomingMusic.volume = immediate ? musicVolume : 0f;
            incomingMusic.Play();
            crossfade = immediate ? 1f : 0f;
            if (immediate && outgoingMusic != null && outgoingMusic != incomingMusic)
                outgoingMusic.Stop();
        }

        private void StartWind()
        {
            AudioClip wind = Choose(valleyWind, ref lastWind);
            if (wind == null || ambienceSource == null)
                return;
            ambienceSource.clip = wind;
            ambienceSource.loop = true;
            ambienceSource.volume = ambienceVolume;
            ambienceSource.Play();
        }

        private static AudioClip Choose(AudioClip[] clips, ref int previousIndex)
        {
            if (clips == null || clips.Length == 0)
                return null;
            int index = clips.Length == 1 ? 0 : UnityEngine.Random.Range(0, clips.Length);
            if (clips.Length > 1 && index == previousIndex)
                index = (index + 1 + UnityEngine.Random.Range(0, clips.Length - 1)) % clips.Length;
            previousIndex = index;
            return clips[index];
        }

        private void EnsureSources()
        {
            if (musicA == null)
                musicA = CreateSource("Music A", false, 0f);
            if (musicB == null)
                musicB = CreateSource("Music B", false, 0f);
            if (ambienceSource == null)
                ambienceSource = CreateSource("Valley Ambience", false, ambienceVolume);
            if (voiceSource == null)
                voiceSource = CreateSource("Voice", false, voiceVolume);
            if (oneShotSource == null)
                oneShotSource = CreateSource("One Shots", false, 1f);
        }

        private AudioSource CreateSource(string sourceName, bool spatial, float volume)
        {
            Transform child = transform.Find(sourceName);
            GameObject host = child != null ? child.gameObject : new GameObject(sourceName);
            if (child == null)
                host.transform.SetParent(transform, false);
            AudioSource source = host.GetComponent<AudioSource>();
            if (source == null)
                source = host.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = spatial ? 1f : 0f;
            source.volume = volume;
            return source;
        }
    }
}

