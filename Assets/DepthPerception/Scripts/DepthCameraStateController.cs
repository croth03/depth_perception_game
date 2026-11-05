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

    public enum DepthJourneyCamera
    {
        ValleyPassage,
        FigureApproach
    }

    public sealed class DepthCameraStateController : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain brain;

        [Header("Side-scroll layer cameras")]
        [SerializeField] private CinemachineCamera realityCamera;
        [SerializeField] private CinemachineCamera hallucinationCamera;
        [SerializeField] private CinemachineCamera lostRealityCamera;

        [Header("Between-layer depth cameras")]
        [SerializeField] private CinemachineCamera valleyDepthCamera;
        [SerializeField] private CinemachineCamera figureDepthCamera;

        [Header("Editable blend timing")]
        [SerializeField] private float sideToDepthDuration = 1.8f;
        [SerializeField] private float depthToSideDuration = 1.7f;
        [SerializeField] private float figureToLostRealityDuration = 2.2f;

        public PerceptionState CurrentState { get; private set; }
        public bool IsDepthTraversal { get; private set; }
        public DepthJourneyCamera CurrentJourney { get; private set; }

        private void Awake()
        {
            SetSideState(PerceptionState.Reality, 0.01f);
        }

        public void SetSideState(PerceptionState state, float durationOverride = -1f)
        {
            float duration = durationOverride > 0f
                ? durationOverride
                : state == PerceptionState.LostReality
                    ? figureToLostRealityDuration
                    : depthToSideDuration;

            CinemachineCamera selected = state == PerceptionState.Reality
                ? realityCamera
                : state == PerceptionState.Hallucination
                    ? hallucinationCamera
                    : lostRealityCamera;

            SelectCamera(selected, duration);
            CurrentState = state;
            IsDepthTraversal = false;
        }

        public void SetDepthTraversal(DepthJourneyCamera journey, float durationOverride = -1f)
        {
            float duration = durationOverride > 0f ? durationOverride : sideToDepthDuration;
            CinemachineCamera selected = journey == DepthJourneyCamera.ValleyPassage
                ? valleyDepthCamera
                : figureDepthCamera;

            SelectCamera(selected, duration);
            CurrentJourney = journey;
            IsDepthTraversal = true;
        }

        public void RestorePresentation(
            PerceptionState perception,
            PlayerMovementMode movementMode,
            DepthJourneyCamera journey,
            float duration)
        {
            if (movementMode == PlayerMovementMode.DepthTraversal)
            {
                CurrentState = perception;
                SetDepthTraversal(journey, duration);
            }
            else
            {
                SetSideState(perception, duration);
            }
        }

        private void SelectCamera(CinemachineCamera selected, float duration)
        {
            if (selected == null || brain == null)
                return;

            brain.DefaultBlend = new CinemachineBlendDefinition(
                CinemachineBlendDefinition.Styles.EaseInOut,
                Mathf.Max(0.01f, duration));

            CinemachineCamera[] cameras =
            {
                realityCamera,
                hallucinationCamera,
                lostRealityCamera,
                valleyDepthCamera,
                figureDepthCamera
            };

            foreach (CinemachineCamera camera in cameras)
            {
                if (camera != null)
                    camera.Priority = camera == selected ? 30 : 10;
            }
        }
    }
}
