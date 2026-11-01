using System.Collections;
using UnityEngine;

namespace DepthPerception
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class PerceptionTransitionTrigger : MonoBehaviour
    {
        [SerializeField] private DepthPlayerController player;
        [SerializeField] private DepthCameraStateController cameraState;
        [SerializeField] private PerceptionState targetState = PerceptionState.Hallucination;

        [Header("Editable transition")]
        [SerializeField] private Vector3 newMovementAxis = Vector3.forward;
        [SerializeField] private float cameraBlendDuration = 2.4f;
        [SerializeField] private float movementBlendDuration = 1.7f;
        [SerializeField] private Color fogColor;
        [SerializeField] private float fogDensity = 0.045f;
        [SerializeField] private float fogBlendDuration = 2f;
        [SerializeField] private Transform checkpoint;

        private bool triggered;

        private void Reset()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            DepthPlayerController found = other.GetComponentInParent<DepthPlayerController>();
            if (triggered || found == null || (player != null && found != player))
                return;

            player = found;
            triggered = true;
            cameraState.SetState(targetState, cameraBlendDuration);
            player.BlendMovementAxis(newMovementAxis, movementBlendDuration);
            if (checkpoint != null)
                player.SetCheckpoint(checkpoint.position);
            StartCoroutine(BlendFog());
        }

        private IEnumerator BlendFog()
        {
            Color startColor = RenderSettings.fogColor;
            float startDensity = RenderSettings.fogDensity;
            float elapsed = 0f;

            while (elapsed < fogBlendDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(
                    0f, 1f, Mathf.Clamp01(elapsed / Mathf.Max(0.01f, fogBlendDuration)));
                RenderSettings.fogColor = Color.Lerp(startColor, fogColor, t);
                RenderSettings.fogDensity = Mathf.Lerp(startDensity, fogDensity, t);
                yield return null;
            }

            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
        }
    }
}
