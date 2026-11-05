using System;
using UnityEngine;

namespace DepthPerception
{
    public sealed class DepthBirdFlock : MonoBehaviour
    {
        [Serializable]
        public sealed class Flight
        {
            public Transform bird;
            public Vector3 start;
            public Vector3 end;
            [Min(4f)] public float cycleDuration = 20f;
            [Min(0.5f)] public float visibleDuration = 6f;
            public float phase;
        }

        [SerializeField] private DepthCameraStateController cameraState;
        [SerializeField] private Flight[] flights = Array.Empty<Flight>();
        [SerializeField] private AudioSource callSource;
        [SerializeField] private AudioClip[] birdCalls = Array.Empty<AudioClip>();
        [SerializeField] private Vector2 callInterval = new Vector2(18f, 34f);
        [SerializeField, Range(0f, 1f)] private float callVolume = 0.22f;

        private int lastCall = -1;
        private float nextCall;

        private void Awake()
        {
            if (cameraState == null)
                cameraState = FindAnyObjectByType<DepthCameraStateController>();
            if (callSource == null)
            {
                callSource = GetComponent<AudioSource>();
                if (callSource == null)
                    callSource = gameObject.AddComponent<AudioSource>();
            }
            callSource.playOnAwake = false;
            callSource.spatialBlend = 0.35f;
            callSource.volume = callVolume;
            ScheduleCall();
        }

        private void Update()
        {
            PerceptionState state = cameraState != null ? cameraState.CurrentState : PerceptionState.Reality;
            AnimateFlights(state);
            if (state != PerceptionState.LostReality && Time.time >= nextCall)
            {
                AudioClip call = ChooseCall();
                if (call != null)
                    callSource.PlayOneShot(call, callVolume);
                ScheduleCall();
            }
        }

        private void AnimateFlights(PerceptionState state)
        {
            float now = Time.time;
            foreach (Flight flight in flights)
            {
                if (flight == null || flight.bird == null)
                    continue;
                float duration = Mathf.Max(4f, flight.cycleDuration);
                float localTime = Mathf.Repeat(now + flight.phase * duration, duration);
                bool visible = state != PerceptionState.LostReality && localTime <= Mathf.Min(duration, flight.visibleDuration);
                flight.bird.gameObject.SetActive(visible);
                if (!visible)
                    continue;
                float t = Mathf.Clamp01(localTime / Mathf.Max(0.1f, flight.visibleDuration));
                if (state == PerceptionState.Hallucination && ((int)(flight.phase * 10f) & 1) == 1)
                    t = 1f - t;
                Vector3 position = Vector3.Lerp(flight.start, flight.end, t);
                position.y += Mathf.Sin(t * Mathf.PI) * 0.65f + Mathf.Sin(Time.time * 3f + flight.phase) * 0.08f;
                flight.bird.localPosition = position;
                Vector3 direction = flight.end - flight.start;
                Vector3 scale = flight.bird.localScale;
                scale.x = Mathf.Abs(scale.x) * (direction.x < 0f ? -1f : 1f) * (state == PerceptionState.Hallucination && t < 0.5f ? -1f : 1f);
                flight.bird.localScale = scale;
            }
        }

        private AudioClip ChooseCall()
        {
            if (birdCalls == null || birdCalls.Length == 0)
                return null;
            int index = birdCalls.Length == 1 ? 0 : UnityEngine.Random.Range(0, birdCalls.Length);
            if (birdCalls.Length > 1 && index == lastCall)
                index = (index + 1 + UnityEngine.Random.Range(0, birdCalls.Length - 1)) % birdCalls.Length;
            lastCall = index;
            return birdCalls[index];
        }

        private void ScheduleCall() => nextCall = Time.time + UnityEngine.Random.Range(Mathf.Max(6f, callInterval.x), Mathf.Max(callInterval.x, callInterval.y));
    }
}
