using System;
using UnityEngine;

namespace DepthPerception
{
    public sealed class DepthAtmosphereStateTreatment : MonoBehaviour
    {
        [SerializeField] private DepthCameraStateController cameraState;
        [SerializeField] private Transform hallucinationRoot;
        [SerializeField] private Transform lostRealityRoot;
        [SerializeField] private Renderer[] hallucinationRenderers = Array.Empty<Renderer>();
        [SerializeField] private Renderer[] lostRealityRenderers = Array.Empty<Renderer>();
        [SerializeField] private Color hallucinationCyan = new Color(0.22f, 0.58f, 0.72f, 1f);
        [SerializeField] private Color hallucinationViolet = new Color(0.48f, 0.34f, 0.72f, 1f);
        [SerializeField] private Color lostMagenta = new Color(0.68f, 0.23f, 0.58f, 1f);
        [SerializeField] private Color lostCyan = new Color(0.18f, 0.62f, 0.76f, 1f);

        private Vector3 hallucinationPosition;
        private Vector3 lostPosition;
        private Quaternion hallucinationRotation;
        private Quaternion lostRotation;
        private MaterialPropertyBlock properties;

        private void Awake()
        {
            if (cameraState == null)
                cameraState = FindAnyObjectByType<DepthCameraStateController>();
            CacheOrigins();
        }

        private void OnEnable() => CacheOrigins();

        private void Update()
        {
            PerceptionState state = cameraState != null ? cameraState.CurrentState : PerceptionState.Reality;
            float time = Time.time;
            if (hallucinationRoot != null)
            {
                float strength = state == PerceptionState.Hallucination ? 1f : state == PerceptionState.LostReality ? 1.35f : 0f;
                hallucinationRoot.localPosition = hallucinationPosition + new Vector3(Mathf.Sin(time * 0.07f) * 0.35f * strength, Mathf.Cos(time * 0.09f) * 0.16f * strength, 0f);
                hallucinationRoot.localRotation = hallucinationRotation * Quaternion.Euler(0f, Mathf.Sin(time * 0.045f) * 0.55f * strength, Mathf.Sin(time * 0.06f) * 0.3f * strength);
            }
            if (lostRealityRoot != null)
            {
                float strength = state == PerceptionState.LostReality ? 1f : 0f;
                lostRealityRoot.localPosition = lostPosition + new Vector3(Mathf.Sin(time * 0.10f) * 0.7f * strength, Mathf.Cos(time * 0.075f) * 0.35f * strength, 0f);
                lostRealityRoot.localRotation = lostRotation * Quaternion.Euler(Mathf.Sin(time * 0.04f) * 0.25f * strength, Mathf.Sin(time * 0.055f) * 1.1f * strength, Mathf.Cos(time * 0.07f) * 0.55f * strength);
            }

            ApplyTint(hallucinationRenderers, Color.Lerp(hallucinationCyan, hallucinationViolet, (Mathf.Sin(time * 0.11f) + 1f) * 0.5f), 0.22f);
            ApplyTint(lostRealityRenderers, Color.Lerp(lostMagenta, lostCyan, (Mathf.Sin(time * 0.085f) + 1f) * 0.5f), 0.38f);
        }

        private void CacheOrigins()
        {
            if (hallucinationRoot != null)
            {
                hallucinationPosition = hallucinationRoot.localPosition;
                hallucinationRotation = hallucinationRoot.localRotation;
            }
            if (lostRealityRoot != null)
            {
                lostPosition = lostRealityRoot.localPosition;
                lostRotation = lostRealityRoot.localRotation;
            }
        }

        private void ApplyTint(Renderer[] renderers, Color tint, float emissionStrength)
        {
            properties ??= new MaterialPropertyBlock();
            foreach (Renderer renderer in renderers)
            {
                if (renderer == null || !renderer.gameObject.activeInHierarchy)
                    continue;
                Color appliedTint = tint;
                Material shared = renderer.sharedMaterial;
                if (shared != null)
                {
                    if (shared.HasProperty("_BaseColor")) appliedTint.a = shared.GetColor("_BaseColor").a;
                    else if (shared.HasProperty("_Color")) appliedTint.a = shared.GetColor("_Color").a;
                }
                renderer.GetPropertyBlock(properties);
                properties.SetColor("_BaseColor", appliedTint);
                properties.SetColor("_Color", appliedTint);
                properties.SetColor("_EmissionColor", tint * emissionStrength);
                renderer.SetPropertyBlock(properties);
            }
        }
    }
}
