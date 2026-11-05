using System;
using System.Collections;
using UnityEngine;

namespace DepthPerception
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class DepthVoiceTrigger : MonoBehaviour
    {
        [SerializeField] private DepthPlayerController player;
        [SerializeField] private DepthAudioDirector audioDirector;
        [SerializeField] private AudioClip voiceClip;
        [SerializeField] private AudioClip[] leadInSounds = Array.Empty<AudioClip>();
        [SerializeField] private bool lockPlayer;
        [SerializeField, Min(0f)] private float delay;
        [SerializeField] private bool oneShot = true;

        private bool consumed;
        private bool running;
        private int lastLeadIn = -1;

        public bool Consumed => consumed;

        private void Reset()
        {
            BoxCollider trigger = GetComponent<BoxCollider>();
            trigger.isTrigger = true;
            trigger.size = new Vector3(3f, 3f, 2f);
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
            StartCoroutine(Play());
        }

        private IEnumerator Play()
        {
            running = true;
            if (audioDirector == null)
                audioDirector = FindAnyObjectByType<DepthAudioDirector>();
            if (lockPlayer && player != null)
                player.SetControlLocked(true);
            if (delay > 0f)
                yield return new WaitForSeconds(delay);
            if (audioDirector != null)
            {
                if (leadInSounds != null && leadInSounds.Length > 0)
                    audioDirector.PlayRandomOneShot(leadInSounds, ref lastLeadIn, 0.6f);
                yield return audioDirector.PlayVoiceAndWait(voiceClip);
            }
            if (lockPlayer && player != null)
                player.SetControlLocked(false);
            running = false;
        }

        private void OnDisable()
        {
            if (running && lockPlayer && player != null)
                player.SetControlLocked(false);
            running = false;
        }
    }
}
