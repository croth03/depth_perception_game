using System;
using UnityEngine;

namespace DepthPerception
{
    public sealed class DepthBackdropRig : MonoBehaviour
    {
        [Serializable]
        public sealed class CloudBand
        {
            public Transform root;
            public Vector3 realityVelocity = new Vector3(0.16f, 0f, 0f);
            public Vector3 hallucinationVelocity = new Vector3(-0.2f, 0.015f, 0f);
            public Vector3 lostRealityVelocity = new Vector3(0.28f, -0.01f, 0.04f);
            public float horizontalWrap = 54f;

            [NonSerialized] public Vector3 origin;
            [NonSerialized] public Vector3 travel;
        }

        [Serializable]
        public sealed class BirdFlight
        {
            public Transform bird;
            public Vector3 start;
            public Vector3 end;
            [Min(4f)] public float cycleDuration = 18f;
            [Min(0.5f)] public float visibleDuration = 5f;
            public float phase;
        }

        [SerializeField] private DepthCameraStateController cameraState;
        [SerializeField] private Renderer panoramaRenderer;
        [SerializeField, Min(0.1f)] private float panoramaStateBlendSpeed = 0.65f;
        [SerializeField] private GameObject hallucinationTreatment;
        [SerializeField] private GameObject lostRealityTreatment;
        [SerializeField] private CloudBand[] cloudBands = Array.Empty<CloudBand>();
        [SerializeField] private BirdFlight[] birdFlights = Array.Empty<BirdFlight>();

        private static readonly int PerceptionStateId = Shader.PropertyToID("_PerceptionState");
        private MaterialPropertyBlock panoramaProperties;
        private float displayedPanoramaState;

        private void Awake()
        {
            CacheOrigins();
            ApplyState();
            SnapPanoramaState();
        }

        private void OnEnable()
        {
            CacheOrigins();
            ApplyState();
            SnapPanoramaState();
        }

        private void Update()
        {
            ApplyState();
            if (!Application.isPlaying)
                return;

            PerceptionState state = cameraState != null
                ? cameraState.CurrentState
                : PerceptionState.Reality;
            UpdatePanoramaState(state);
            AnimateClouds(state);
            AnimateBirds(state);
        }

        private void CacheOrigins()
        {
            foreach (CloudBand band in cloudBands)
                if (band != null && band.root != null)
                {
                    band.origin = band.root.localPosition;
                    band.travel = Vector3.zero;
                }
        }

        private void SnapPanoramaState()
        {
            PerceptionState state = cameraState != null
                ? cameraState.CurrentState
                : PerceptionState.Reality;
            displayedPanoramaState = (float)state;
            ApplyPanoramaProperties();
        }

        private void UpdatePanoramaState(PerceptionState state)
        {
            float target = (float)state;
            displayedPanoramaState = Application.isPlaying
                ? Mathf.MoveTowards(displayedPanoramaState, target, panoramaStateBlendSpeed * Time.deltaTime)
                : target;
            ApplyPanoramaProperties();
        }

        private void ApplyPanoramaProperties()
        {
            if (panoramaRenderer == null)
                return;

            panoramaProperties ??= new MaterialPropertyBlock();
            panoramaRenderer.GetPropertyBlock(panoramaProperties);
            panoramaProperties.SetFloat(PerceptionStateId, displayedPanoramaState);
            panoramaRenderer.SetPropertyBlock(panoramaProperties);
        }

        private void ApplyState()
        {
            PerceptionState state = cameraState != null
                ? cameraState.CurrentState
                : PerceptionState.Reality;

            if (hallucinationTreatment != null)
                hallucinationTreatment.SetActive(state != PerceptionState.Reality);
            if (lostRealityTreatment != null)
                lostRealityTreatment.SetActive(state == PerceptionState.LostReality);
        }

        private void AnimateClouds(PerceptionState state)
        {
            for (int index = 0; index < cloudBands.Length; index++)
            {
                CloudBand band = cloudBands[index];
                if (band == null || band.root == null)
                    continue;

                Vector3 velocity = state == PerceptionState.Reality
                    ? band.realityVelocity
                    : state == PerceptionState.Hallucination
                        ? band.hallucinationVelocity
                        : band.lostRealityVelocity;
                band.travel += velocity * Time.deltaTime;

                float wrap = Mathf.Max(8f, band.horizontalWrap);
                if (band.travel.x > wrap) band.travel.x -= wrap * 2f;
                else if (band.travel.x < -wrap) band.travel.x += wrap * 2f;

                float phase = index * 1.7f;
                Vector3 wave = Vector3.zero;
                if (state == PerceptionState.Hallucination)
                {
                    wave.y = Mathf.Sin(Time.time * 0.13f + phase) * 0.16f;
                    wave.z = Mathf.Cos(Time.time * 0.10f + phase) * 0.10f;
                }
                else if (state == PerceptionState.LostReality)
                {
                    wave.y = Mathf.Sin(Time.time * 0.18f + phase) * 0.38f;
                    wave.z = Mathf.Cos(Time.time * 0.14f + phase) * 0.28f;
                }

                band.root.localPosition = band.origin + band.travel + wave;
            }
        }

        private void AnimateBirds(PerceptionState state)
        {
            float now = Time.time;
            foreach (BirdFlight flight in birdFlights)
            {
                if (flight == null || flight.bird == null)
                    continue;

                float duration = Mathf.Max(4f, flight.cycleDuration);
                float localTime = Mathf.Repeat(now + flight.phase * duration, duration);
                bool visible = state == PerceptionState.Reality
                    && localTime <= Mathf.Min(duration, flight.visibleDuration);
                flight.bird.gameObject.SetActive(visible);
                if (!visible)
                    continue;

                float t = Mathf.Clamp01(localTime / Mathf.Max(0.1f, flight.visibleDuration));
                Vector3 position = Vector3.Lerp(flight.start, flight.end, t);
                position.y += Mathf.Sin(t * Mathf.PI) * 0.7f;
                flight.bird.localPosition = position;

                Vector3 direction = flight.end - flight.start;
                Vector3 scale = flight.bird.localScale;
                scale.x = Mathf.Abs(scale.x) * (direction.x < 0f ? -1f : 1f);
                flight.bird.localScale = scale;
            }
        }
    }
}
