using System.Collections;
using UnityEngine;

namespace DepthPerception
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class FigureEncounterSequence : MonoBehaviour
    {
        [SerializeField] private DepthPlayerController player;
        [SerializeField] private DepthCameraStateController cameraState;
        [SerializeField] private Transform figure;
        [SerializeField] private Transform lostRealitySpawn;
        [SerializeField] private Transform lostRecoveryCheckpoint;

        [Header("Editable encounter")]
        [SerializeField] private float approachDuration = 2.6f;
        [SerializeField] private float lostCameraBlendDuration = 2.2f;
        [SerializeField] private float lostCameraLeadTime = 0.7f;
        [SerializeField] private float controlReturnDelay = 0.5f;
        [SerializeField] private float lostSideLaneZ = 44f;
        [SerializeField] private Color lostFogColor;
        [SerializeField] private float lostFogDensity = 0.075f;

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
            StartCoroutine(RunEncounter());
        }

        private IEnumerator RunEncounter()
        {
            player.SetControlLocked(true);

            Vector3 figureStart = figure.position;
            Color fogStart = RenderSettings.fogColor;
            float densityStart = RenderSettings.fogDensity;
            float elapsed = 0f;

            while (elapsed < approachDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(
                    0f,
                    1f,
                    Mathf.Clamp01(elapsed / Mathf.Max(0.01f, approachDuration)));
                Vector3 approachPoint = player.transform.position
                    + Vector3.up * 1.1f
                    + Vector3.forward * Mathf.Lerp(0.8f, -0.35f, t);
                figure.position = Vector3.Lerp(figureStart, approachPoint, t);
                RenderSettings.fogColor = Color.Lerp(fogStart, lostFogColor, t * 0.7f);
                RenderSettings.fogDensity = Mathf.Lerp(densityStart, lostFogDensity, t * 0.85f);
                yield return null;
            }

            cameraState.SetSideState(PerceptionState.LostReality, lostCameraBlendDuration);
            yield return new WaitForSeconds(lostCameraLeadTime);

            player.Teleport(lostRealitySpawn.position);
            player.SetSideScrollMode(lostSideLaneZ);
            player.SetControlLocked(true);

            Vector3 recoveryPosition = lostRecoveryCheckpoint != null
                ? lostRecoveryCheckpoint.position
                : lostRealitySpawn.position;
            player.SetCheckpoint(
                recoveryPosition,
                PerceptionState.LostReality,
                PlayerMovementMode.SideScroll);

            figure.gameObject.SetActive(false);
            RenderSettings.fogColor = lostFogColor;
            RenderSettings.fogDensity = lostFogDensity;

            yield return new WaitForSeconds(controlReturnDelay);
            player.SetControlLocked(false);
        }
    }
}
