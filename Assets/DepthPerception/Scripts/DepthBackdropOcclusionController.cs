using UnityEngine;

namespace DepthPerception
{
    public sealed class DepthBackdropOcclusionController : MonoBehaviour
    {
        [SerializeField] private DepthCameraStateController cameraState;
        [SerializeField] private Renderer[] realityPassageRenderers;

        private PerceptionState lastState = (PerceptionState)(-1);

        private void OnEnable()
        {
            ApplyVisibility(true);
        }

        private void Update()
        {
            ApplyVisibility(false);
        }

        private void ApplyVisibility(bool force)
        {
            PerceptionState state = cameraState != null
                ? cameraState.CurrentState
                : PerceptionState.Reality;
            if (!force && state == lastState)
                return;

            lastState = state;
            bool visible = state == PerceptionState.Reality;
            if (realityPassageRenderers == null)
                return;

            foreach (Renderer passageRenderer in realityPassageRenderers)
                if (passageRenderer != null)
                    passageRenderer.enabled = visible;
        }
    }
}
