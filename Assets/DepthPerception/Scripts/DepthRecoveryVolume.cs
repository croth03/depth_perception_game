using UnityEngine;

namespace DepthPerception
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class DepthRecoveryVolume : MonoBehaviour
    {
        [SerializeField] private DepthCameraStateController cameraState;

        private void Reset()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            DepthPlayerController player = other.GetComponentInParent<DepthPlayerController>();
            if (player != null)
                player.RecoverToCheckpoint(cameraState);
        }
    }
}

