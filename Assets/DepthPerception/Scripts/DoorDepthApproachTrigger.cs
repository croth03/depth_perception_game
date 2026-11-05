using System.Collections;
using UnityEngine;

namespace DepthPerception
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class DoorDepthApproachTrigger : MonoBehaviour
    {
        [SerializeField] private DepthPlayerController player;
        [SerializeField] private DepthCameraStateController cameraState;
        [SerializeField] private Transform checkpoint;
        [SerializeField, Min(0.05f)] private float cameraBlendDuration = 1.35f;
        [SerializeField, Min(0f)] private float controlLockDuration = 0.65f;
        [SerializeField] private float corridorCenterX = 40.3f;
        [SerializeField, Min(0.5f)] private float corridorHalfWidth = 1.8f;

        private bool triggered;

        private void Reset()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnValidate()
        {
            BoxCollider box = GetComponent<BoxCollider>();
            if (box != null)
                box.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            DepthPlayerController found = other.GetComponentInParent<DepthPlayerController>();
            if (triggered || found == null || (player != null && found != player))
                return;

            player = found;
            triggered = true;
            StartCoroutine(TransitionToDoorApproach());
        }

        private IEnumerator TransitionToDoorApproach()
        {
            player.SetControlLocked(true);
            cameraState.SetDepthTraversal(DepthJourneyCamera.FigureApproach, cameraBlendDuration);

            yield return new WaitForSeconds(controlLockDuration);

            player.SetDepthTraversalMode(Vector3.forward, corridorCenterX, corridorHalfWidth);
            player.SetCheckpoint(
                checkpoint != null ? checkpoint.position : player.transform.position,
                cameraState.CurrentState,
                PlayerMovementMode.DepthTraversal,
                DepthJourneyCamera.FigureApproach);
            player.SetControlLocked(false);
        }
    }
}
