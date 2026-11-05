using System;
using UnityEngine;

namespace DepthPerception
{
    public sealed class DepthCloudRig : MonoBehaviour
    {
        [Serializable]
        public sealed class Layer
        {
            public Transform root;
            public Vector3 realityVelocity = new Vector3(0.12f, 0f, 0f);
            public Vector3 hallucinationVelocity = new Vector3(-0.15f, 0.01f, 0f);
            public Vector3 lostRealityVelocity = new Vector3(0.2f, -0.01f, 0.025f);
            public float horizontalWrap = 60f;
            public float hallucinationRotationSpeed = 0.08f;
            public float lostRealityRotationSpeed = 0.24f;
            [NonSerialized] public Vector3 origin;
            [NonSerialized] public Quaternion rotation;
            [NonSerialized] public Vector3 travel;
        }

        [SerializeField] private DepthCameraStateController cameraState;
        [SerializeField] private Layer[] layers = Array.Empty<Layer>();
        [SerializeField] private Renderer[] cloudRenderers = Array.Empty<Renderer>();
        [SerializeField] private Color realityTint = new Color(0.58f, 0.68f, 0.78f, 0.18f);
        [SerializeField] private Color hallucinationTint = new Color(0.42f, 0.48f, 0.76f, 0.22f);
        [SerializeField] private Color lostRealityTint = new Color(0.58f, 0.28f, 0.72f, 0.27f);
        [SerializeField, Min(0.05f)] private float tintBlendSpeed = 0.35f;

        private MaterialPropertyBlock properties;
        private Color displayedTint;

        private void Awake()
        {
            if (cameraState == null)
                cameraState = FindAnyObjectByType<DepthCameraStateController>();
            CacheOrigins();
            displayedTint = realityTint;
        }

        private void OnEnable()
        {
            CacheOrigins();
        }

        private void Update()
        {
            if (cameraState == null)
                cameraState = FindAnyObjectByType<DepthCameraStateController>();
            PerceptionState state = cameraState != null ? cameraState.CurrentState : PerceptionState.Reality;
            AnimateLayers(state);
            AnimateTint(state);
        }

        private void CacheOrigins()
        {
            foreach (Layer layer in layers)
            {
                if (layer == null || layer.root == null)
                    continue;
                layer.origin = layer.root.localPosition;
                layer.rotation = layer.root.localRotation;
                layer.travel = Vector3.zero;
            }
        }

        private void AnimateLayers(PerceptionState state)
        {
            for (int index = 0; index < layers.Length; index++)
            {
                Layer layer = layers[index];
                if (layer == null || layer.root == null)
                    continue;
                Vector3 velocity = state == PerceptionState.Reality
                    ? layer.realityVelocity
                    : state == PerceptionState.Hallucination ? layer.hallucinationVelocity : layer.lostRealityVelocity;
                layer.travel += velocity * Time.deltaTime;
                float wrap = Mathf.Max(8f, layer.horizontalWrap);
                if (layer.travel.x > wrap) layer.travel.x -= wrap * 2f;
                else if (layer.travel.x < -wrap) layer.travel.x += wrap * 2f;

                float phase = index * 1.71f;
                float strength = state == PerceptionState.Reality ? 0.03f : state == PerceptionState.Hallucination ? 0.14f : 0.32f;
                Vector3 wave = new Vector3(0f, Mathf.Sin(Time.time * 0.11f + phase) * strength, Mathf.Cos(Time.time * 0.08f + phase) * strength * 0.65f);
                layer.root.localPosition = layer.origin + layer.travel + wave;
                float rotationSpeed = state == PerceptionState.Hallucination ? layer.hallucinationRotationSpeed : state == PerceptionState.LostReality ? layer.lostRealityRotationSpeed : 0f;
                layer.root.localRotation = layer.rotation * Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time * 0.09f + phase) * rotationSpeed * 12f);
            }
        }

        private void AnimateTint(PerceptionState state)
        {
            Color target = state == PerceptionState.Reality ? realityTint : state == PerceptionState.Hallucination ? hallucinationTint : lostRealityTint;
            if (state == PerceptionState.LostReality)
            {
                float pulse = (Mathf.Sin(Time.time * 0.12f) + 1f) * 0.5f;
                target = Color.Lerp(lostRealityTint, new Color(0.2f, 0.62f, 0.78f, lostRealityTint.a), pulse * 0.32f);
            }
            displayedTint = Color.Lerp(displayedTint, target, 1f - Mathf.Exp(-tintBlendSpeed * Time.deltaTime));
            properties ??= new MaterialPropertyBlock();
            foreach (Renderer renderer in cloudRenderers)
            {
                if (renderer == null)
                    continue;
                renderer.GetPropertyBlock(properties);
                properties.SetColor("_BaseColor", displayedTint);
                properties.SetColor("_Color", displayedTint);
                renderer.SetPropertyBlock(properties);
            }
        }
    }
}
