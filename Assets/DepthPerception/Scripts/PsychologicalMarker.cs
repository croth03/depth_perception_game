using System;
using System.Collections;
using UnityEngine;

namespace DepthPerception
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class PsychologicalMarker : MonoBehaviour
    {
    [SerializeField] private Renderer symbolGlowRenderer;
        private Coroutine symbolGlowRoutine;
        private MaterialPropertyBlock symbolGlowBlock;
        private float symbolGlowAmount;

        public enum MovementAfterEncounter
        {
            Preserve,
            SideScroll,
            DepthTraversal
        }

        [Header("Runtime references")]
        [SerializeField] private DepthPlayerController player;
        [SerializeField] private DepthCameraStateController cameraState;
        [SerializeField] private DepthAudioDirector audioDirector;

        [Header("Encounter audio")]
        [SerializeField] private AudioClip voiceClip;
        [SerializeField] private AudioClip[] transitionSounds = Array.Empty<AudioClip>();
        [SerializeField, Min(0f)] private float delayBeforeVoice = 0.2f;
        [SerializeField, Min(0f)] private float delayAfterVoice = 0.25f;

        [Header("Optional perception/camera transition")]
        [SerializeField] private bool changePresentation;
        [SerializeField] private PerceptionState targetState = PerceptionState.Hallucination;
        [SerializeField] private bool useDepthCamera;
        [SerializeField] private DepthJourneyCamera depthCamera = DepthJourneyCamera.ValleyPassage;
        [SerializeField, Min(0.01f)] private float cameraBlendDuration = 1f;

        [Header("Optional movement result")]
        [SerializeField] private MovementAfterEncounter movementAfter = MovementAfterEncounter.Preserve;
        [SerializeField] private float sideLaneZ;
        [SerializeField] private Vector3 depthForward = Vector3.forward;
        [SerializeField] private float corridorCenterX;
        [SerializeField, Min(0.5f)] private float corridorHalfWidth = 2f;

        [Header("Control")]
        [SerializeField] private bool lockPlayerDuringVoice = true;
        [SerializeField] private bool oneShot = true;

        private bool consumed;
        private bool running;
        private int lastTransition = -1;

        public bool Consumed => consumed;

        private void Reset()
        {
            BoxCollider trigger = GetComponent<BoxCollider>();
            trigger.isTrigger = true;
            trigger.size = new Vector3(3f, 3f, 3f);
        }

        private void OnValidate()
        {
            BoxCollider trigger = GetComponent<BoxCollider>();
            if (trigger != null)
                trigger.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            DepthPlayerController found = other.GetComponentInParent<DepthPlayerController>();
            if (found == null || running || (oneShot && consumed))
                return;
            player = found;
            consumed = true;
            StartCoroutine(RunEncounter());
        }

        private IEnumerator RunEncounter()
        {
            running = true;
            if (cameraState == null)
                cameraState = FindAnyObjectByType<DepthCameraStateController>();
            if (audioDirector == null)
                audioDirector = FindAnyObjectByType<DepthAudioDirector>();
            if (lockPlayerDuringVoice && player != null)
                player.SetControlLocked(true);

            if (delayBeforeVoice > 0f)
                yield return new WaitForSeconds(delayBeforeVoice);

            if (audioDirector != null)
            {
                if (transitionSounds != null && transitionSounds.Length > 0)
                    audioDirector.PlayRandomOneShot(transitionSounds, ref lastTransition, 0.65f);
                if (symbolGlowRenderer != null)
                {
                    if (symbolGlowRoutine != null) StopCoroutine(symbolGlowRoutine);
                    symbolGlowRoutine = StartCoroutine(TrackSymbolGlow(audioDirector));
                }
                yield return audioDirector.PlayVoiceAndWait(voiceClip, delayAfterVoice);
            }
            else
            {
                float duration = voiceClip != null ? voiceClip.length : 0f;
                if (duration + delayAfterVoice > 0f)
                    yield return new WaitForSecondsRealtime(duration + delayAfterVoice);
            }

            ApplyConfiguredTransition();
            if (lockPlayerDuringVoice && player != null)
                player.SetControlLocked(false);
            running = false;
        }

        private void ApplyConfiguredTransition()
        {
            if (changePresentation && cameraState != null)
            {
                if (useDepthCamera)
                    cameraState.SetDepthTraversal(depthCamera, cameraBlendDuration);
                else
                    cameraState.SetSideState(targetState, cameraBlendDuration);
            }

            if (player == null)
                return;
            if (movementAfter == MovementAfterEncounter.SideScroll)
                player.SetSideScrollMode(sideLaneZ);
            else if (movementAfter == MovementAfterEncounter.DepthTraversal)
                player.SetDepthTraversalMode(depthForward, corridorCenterX, corridorHalfWidth);
        }


        private IEnumerator TrackSymbolGlow(DepthAudioDirector director)
        {
            float wait = 0f;
            while (director != null && !director.IsVoicePlaying && wait < 2f) { wait += Time.unscaledDeltaTime; yield return null; }
            if (director == null || !director.IsVoicePlaying) { yield return FadeSymbolGlow(0f); yield break; }
            yield return FadeSymbolGlow(1f);
            while (director != null && director.IsVoicePlaying)
            {
                ApplySymbolGlow(0.92f + Mathf.Sin(Time.unscaledTime * 2.4f) * 0.08f);
                yield return null;
            }
            yield return FadeSymbolGlow(0f);
            symbolGlowRoutine = null;
        }

        private IEnumerator FadeSymbolGlow(float target)
        {
            while (!Mathf.Approximately(symbolGlowAmount, target))
            {
                ApplySymbolGlow(Mathf.MoveTowards(symbolGlowAmount, target, Time.unscaledDeltaTime / 0.45f));
                yield return null;
            }
        }

        private void ApplySymbolGlow(float value)
        {
            symbolGlowAmount = Mathf.Clamp01(value);
            if (symbolGlowRenderer == null) return;
            symbolGlowBlock ??= new MaterialPropertyBlock();
            Color c = targetState == PerceptionState.Hallucination ? new Color(0.46f,0.43f,0.72f,1f) : targetState == PerceptionState.LostReality ? new Color(0.58f,0.27f,0.48f,1f) : new Color(0.55f,0.76f,0.82f,1f);
            Color visible = new Color(c.r,c.g,c.b,symbolGlowAmount*0.7f);
            symbolGlowBlock.SetColor("_BaseColor",visible);
            symbolGlowBlock.SetColor("_Color",visible);
            symbolGlowBlock.SetColor("_EmissionColor",c*(symbolGlowAmount*0.72f));
            symbolGlowRenderer.SetPropertyBlock(symbolGlowBlock);
            symbolGlowRenderer.enabled = symbolGlowAmount > 0.002f;
        }

        private void OnDisable()
        {
            if (running && lockPlayerDuringVoice && player != null)
                player.SetControlLocked(false);
            running = false;
            if (symbolGlowRoutine != null) StopCoroutine(symbolGlowRoutine);
            symbolGlowRoutine = null;
            ApplySymbolGlow(0f);
        }
    }
}

