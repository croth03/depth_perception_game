using System.Collections;
using UnityEngine;

namespace DepthPerception
{
    public enum PerceptionTransitionType
    {
        EnterDepthTraversal,
        EnterSideScrollLayer
    }

    [RequireComponent(typeof(BoxCollider))]
    public sealed class PerceptionTransitionTrigger : MonoBehaviour
    {
        [SerializeField] private DepthPlayerController player;
        [SerializeField] private DepthCameraStateController cameraState;
        [SerializeField] private PerceptionTransitionType transitionType;
        [SerializeField] private PerceptionState targetState = PerceptionState.Hallucination;
        [SerializeField] private DepthJourneyCamera depthCamera = DepthJourneyCamera.ValleyPassage;

        [Header("Movement destination")]
        [SerializeField] private float sideLaneZ;
        [SerializeField] private Vector3 depthForward = Vector3.forward;
        [SerializeField] private float depthCorridorCenterX = 2.2f;
        [SerializeField] private float depthCorridorHalfWidth = 2.3f;
        [SerializeField] private Transform checkpoint;

        [Header("Editable transition")]
        [SerializeField] private float cameraBlendDuration = 0.8f;
        [SerializeField] private float controlLockDuration = 0.8f;
        [SerializeField] private Color fogColor;
        [SerializeField] private float fogDensity = 0.045f;
        [SerializeField] private float fogBlendDuration = 2f;

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
            StartCoroutine(RunTransition());
        }

        private IEnumerator RunTransition()
        {
            player.SetControlLocked(true);
            StartCoroutine(BlendFog());

            if (transitionType == PerceptionTransitionType.EnterDepthTraversal)
                cameraState.SetDepthTraversal(depthCamera, cameraBlendDuration);
            else
                cameraState.SetSideState(targetState, cameraBlendDuration);

            float transitionLock = transitionType == PerceptionTransitionType.EnterDepthTraversal
                ? Mathf.Max(controlLockDuration, cameraBlendDuration)
                : controlLockDuration;
            yield return new WaitForSeconds(transitionLock);

            Vector3 safePosition = checkpoint != null ? checkpoint.position : player.transform.position;
            if (transitionType == PerceptionTransitionType.EnterDepthTraversal)
            {
                player.SetDepthTraversalMode(
                    depthForward,
                    depthCorridorCenterX,
                    depthCorridorHalfWidth);
                player.SetCheckpoint(
                    player.transform.position,
                    cameraState.CurrentState,
                    PlayerMovementMode.DepthTraversal,
                    depthCamera);
            }
            else
            {
                player.SetSideScrollMode(sideLaneZ);
                player.SetCheckpoint(
                    safePosition,
                    targetState,
                    PlayerMovementMode.SideScroll);
            }

            player.SetControlLocked(false);

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
                    0f,
                    1f,
                    Mathf.Clamp01(elapsed / Mathf.Max(0.01f, fogBlendDuration)));
                RenderSettings.fogColor = Color.Lerp(startColor, fogColor, t);
                RenderSettings.fogDensity = Mathf.Lerp(startDensity, fogDensity, t);
                yield return null;
            }

            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
        }
    }
}
