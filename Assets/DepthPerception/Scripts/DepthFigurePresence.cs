using System;
using UnityEngine;

namespace DepthPerception
{
    public sealed class DepthFigurePresence : MonoBehaviour
    {
        [SerializeField] private Renderer[] renderers = Array.Empty<Renderer>();
        [SerializeField] private AudioSource presenceSource;
        [SerializeField] private AudioClip[] presenceClips = Array.Empty<AudioClip>();
        [SerializeField] private Vector2 presenceInterval = new Vector2(11f, 20f);
        [SerializeField, Range(0.05f, 1f)] private float baseOpacity = 0.58f;
        [SerializeField, Range(0f, 0.3f)] private float opacityFluctuation = 0.07f;
        [SerializeField, Min(0f)] private float swayAmount = 0.035f;
        [SerializeField, Min(0f)] private float swaySpeed = 0.28f;

        private Vector3 origin;
        private MaterialPropertyBlock properties;
        private int lastPresence = -1;
        private float nextPresence;

        private void Awake()
        {
            if (renderers == null || renderers.Length == 0)
                renderers = GetComponentsInChildren<Renderer>(true);
            if (presenceSource == null)
            {
                presenceSource = GetComponent<AudioSource>();
                if (presenceSource == null)
                    presenceSource = gameObject.AddComponent<AudioSource>();
            }
            presenceSource.playOnAwake = false;
            presenceSource.loop = false;
            presenceSource.spatialBlend = 0.65f;
            presenceSource.volume = 0.28f;
            presenceSource.minDistance = 4f;
            presenceSource.maxDistance = 45f;
            origin = transform.localPosition;
            SchedulePresence();
        }

        private void Update()
        {
            float wave = Mathf.Sin(Time.time * swaySpeed * Mathf.PI * 2f);
            transform.localPosition = origin + new Vector3(wave * swayAmount, Mathf.Sin(Time.time * swaySpeed * 0.73f) * swayAmount * 0.5f, 0f);
            float alpha = Mathf.Clamp01(baseOpacity + wave * opacityFluctuation);
            Color tint = new Color(0.055f, 0.075f, 0.12f, alpha);
            Color emission = new Color(0.025f, 0.12f, 0.16f, 1f) * (0.35f + (wave + 1f) * 0.08f);
            properties ??= new MaterialPropertyBlock();
            foreach (Renderer renderer in renderers)
            {
                if (renderer == null)
                    continue;
                renderer.GetPropertyBlock(properties);
                properties.SetColor("_BaseColor", tint);
                properties.SetColor("_Color", tint);
                properties.SetColor("_EmissionColor", emission);
                renderer.SetPropertyBlock(properties);
            }

            if (Time.time >= nextPresence && presenceSource != null && !presenceSource.isPlaying)
            {
                AudioClip clip = ChoosePresence();
                if (clip != null)
                    presenceSource.PlayOneShot(clip, 0.28f);
                SchedulePresence();
            }
        }

        private AudioClip ChoosePresence()
        {
            if (presenceClips == null || presenceClips.Length == 0)
                return null;
            int index = presenceClips.Length == 1 ? 0 : UnityEngine.Random.Range(0, presenceClips.Length);
            if (presenceClips.Length > 1 && index == lastPresence)
                index = (index + 1 + UnityEngine.Random.Range(0, presenceClips.Length - 1)) % presenceClips.Length;
            lastPresence = index;
            return presenceClips[index];
        }

        private void SchedulePresence() => nextPresence = Time.time + UnityEngine.Random.Range(Mathf.Max(5f, presenceInterval.x), Mathf.Max(presenceInterval.x, presenceInterval.y));
    }
}
