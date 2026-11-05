using System;
using UnityEngine;

namespace DepthPerception
{
    public sealed class DepthPlayerAudio : MonoBehaviour
    {
        [SerializeField] private DepthPlayerController controller;
        [SerializeField] private DepthAudioDirector audioDirector;
        [SerializeField] private AudioSource footstepSource;
        [SerializeField] private AudioSource bodySource;

        [Header("Variation pools")]
        [SerializeField] private AudioClip[] walkFootsteps = Array.Empty<AudioClip>();
        [SerializeField] private AudioClip[] runFootsteps = Array.Empty<AudioClip>();
        [SerializeField] private AudioClip[] jumpSounds = Array.Empty<AudioClip>();
        [SerializeField] private AudioClip[] landingSounds = Array.Empty<AudioClip>();
        [SerializeField] private AudioClip[] clothingSounds = Array.Empty<AudioClip>();
        [SerializeField] private AudioClip[] idleBreaths = Array.Empty<AudioClip>();
        [SerializeField] private AudioClip[] walkingBreaths = Array.Empty<AudioClip>();

        [Header("Tuning")]
        [SerializeField, Range(0f, 1f)] private float footstepVolume = 0.28f;
        [SerializeField, Range(0f, 1f)] private float bodyVolume = 0.38f;
        [SerializeField, Min(0.1f)] private float minimumAirTimeForLanding = 0.18f;
        [SerializeField] private Vector2 breathInterval = new Vector2(16f, 28f);
        [SerializeField] private Vector2 movementTakeInterval = new Vector2(4f, 7f);

        private bool wasGrounded;
        private float airborneTime;
        private float nextBreath;
        private float nextMovementTake;
        private bool usingRunPool;
        private int lastWalk = -1;
        private int lastRun = -1;
        private int lastJump = -1;
        private int lastLanding = -1;
        private int lastClothing = -1;
        private int lastIdleBreath = -1;
        private int lastWalkingBreath = -1;

        private void Awake()
        {
            if (controller == null)
                controller = GetComponent<DepthPlayerController>();
            if (audioDirector == null)
                audioDirector = FindAnyObjectByType<DepthAudioDirector>();
            if (footstepSource == null)
                footstepSource = CreateSource("Footstep Audio", footstepVolume);
            if (bodySource == null)
                bodySource = CreateSource("Body Audio", bodyVolume);
            wasGrounded = controller != null && controller.IsGrounded;
            ScheduleBreath();
            ScheduleMovementTake();
        }

        private void OnEnable()
        {
            if (controller == null)
                controller = GetComponent<DepthPlayerController>();
            if (controller != null)
                controller.Jumped += HandleJumped;
        }

        private void OnDisable()
        {
            if (controller != null)
                controller.Jumped -= HandleJumped;
            if (footstepSource != null)
                footstepSource.Stop();
        }

        private void Update()
        {
            if (controller == null)
                return;

            bool grounded = controller.IsGrounded;
            float speed = controller.NormalizedSpeed;
            bool moving = grounded && controller.CurrentMode != PlayerMovementMode.ControlLocked && speed > 0.12f;

            if (!grounded)
                airborneTime += Time.deltaTime;
            else if (!wasGrounded)
            {
                if (airborneTime >= minimumAirTimeForLanding)
                    PlayOneShot(landingSounds, ref lastLanding, bodyVolume);
                airborneTime = 0f;
            }
            wasGrounded = grounded;

            UpdateFootsteps(moving, speed);
            UpdateBreathing(moving);
        }

        private void UpdateFootsteps(bool moving, float speed)
        {
            if (!moving)
            {
                if (footstepSource != null && footstepSource.isPlaying)
                    footstepSource.Stop();
                return;
            }

            bool run = speed >= 0.78f;
            if (footstepSource == null)
                return;
            if (!footstepSource.isPlaying || run != usingRunPool || Time.time >= nextMovementTake)
            {
                AudioClip clip = run ? Choose(runFootsteps, ref lastRun) : Choose(walkFootsteps, ref lastWalk);
                if (clip != null)
                {
                    footstepSource.Stop();
                    footstepSource.clip = clip;
                    footstepSource.loop = true;
                    footstepSource.volume = footstepVolume;
                    footstepSource.pitch = run ? 1.05f : 0.86f;
                    footstepSource.time = clip.length > 1f ? UnityEngine.Random.Range(0f, clip.length - 0.5f) : 0f;
                    footstepSource.Play();
                    usingRunPool = run;
                    ScheduleMovementTake();
                    if (UnityEngine.Random.value < 0.2f)
                        PlayOneShot(clothingSounds, ref lastClothing, bodyVolume * 0.32f);
                }
            }
        }

        private void UpdateBreathing(bool moving)
        {
            if (Time.time < nextBreath || (audioDirector != null && audioDirector.IsVoicePlaying))
                return;
            AudioClip[] pool = moving ? walkingBreaths : idleBreaths;
            ref int previous = ref (moving ? ref lastWalkingBreath : ref lastIdleBreath);
            PlayOneShot(pool, ref previous, bodyVolume * 0.28f);
            ScheduleBreath();
        }

        private void HandleJumped(PlayerJumpStyle unused)
        {
            PlayOneShot(jumpSounds, ref lastJump, bodyVolume);
            if (footstepSource != null)
                footstepSource.Stop();
        }

        private void PlayOneShot(AudioClip[] clips, ref int previous, float volume)
        {
            AudioClip clip = Choose(clips, ref previous);
            if (clip != null && bodySource != null)
                bodySource.PlayOneShot(clip, Mathf.Clamp01(volume));
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

        private AudioSource CreateSource(string sourceName, float volume)
        {
            Transform child = transform.Find(sourceName);
            GameObject host = child != null ? child.gameObject : new GameObject(sourceName);
            if (child == null)
                host.transform.SetParent(transform, false);
            AudioSource source = host.GetComponent<AudioSource>();
            if (source == null)
                source = host.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = 0f;
            source.volume = volume;
            return source;
        }

        private void ScheduleBreath() => nextBreath = Time.time + UnityEngine.Random.Range(Mathf.Max(5f, breathInterval.x), Mathf.Max(breathInterval.x, breathInterval.y));
        private void ScheduleMovementTake() => nextMovementTake = Time.time + UnityEngine.Random.Range(Mathf.Max(2f, movementTakeInterval.x), Mathf.Max(movementTakeInterval.x, movementTakeInterval.y));
    }
}
