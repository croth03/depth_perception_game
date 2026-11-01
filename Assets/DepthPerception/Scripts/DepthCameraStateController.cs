using UnityEngine;
using Unity.Cinemachine;

namespace DepthPerception
{
    public enum PerceptionState
    {
        Reality,
        Hallucination,
        LostReality
    }

    public sealed class DepthCameraStateController : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain brain;
        [SerializeField] private CinemachineCamera realityCamera;
        [SerializeField] private CinemachineCamera hallucinationCamera;
        [SerializeField] private CinemachineCamera lostRealityCamera;

        [Header("Editable blend timing")]
        [SerializeField] private float realityToHallucinationDuration = 2.4f;
        [SerializeField] private float figureToLostRealityDuration = 2.8f;

        public PerceptionState CurrentState { get; private set; }

        private void Awake()
        {
            SetState(PerceptionState.Reality, 0.01f);
        }

        public void SetState(PerceptionState state, float durationOverride = -1f)
        {
            float duration = durationOverride > 0f
                ? durationOverride
                : state == PerceptionState.Hallucination
                    ? realityToHallucinationDuration
                    : figureToLostRealityDuration;

            brain.DefaultBlend = new CinemachineBlendDefinition(
                CinemachineBlendDefinition.Styles.EaseInOut, duration);

            realityCamera.Priority = state == PerceptionState.Reality ? 30 : 10;
            hallucinationCamera.Priority = state == PerceptionState.Hallucination ? 30 : 10;
            lostRealityCamera.Priority = state == PerceptionState.LostReality ? 30 : 10;
            CurrentState = state;

        }
    }
}
