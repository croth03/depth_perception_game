using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DepthPerception
{
    public enum PlayerMovementMode
    {
        SideScroll,
        DepthTraversal,
        ControlLocked
    }

    public enum PlayerJumpStyle
    {
        Stationary,
        Walking,
        Running
    }

    [RequireComponent(typeof(CharacterController))]
    public sealed class DepthPlayerController : MonoBehaviour
    {
        [Header("Side-scroll movement")]
        [SerializeField] private float sideMovementSpeed = 5.5f;
        [SerializeField] private float sideLaneCorrectionSpeed = 7f;
        [SerializeField] private float sideLaneZ;

        [Header("Depth traversal movement")]
        [SerializeField] private float depthForwardSpeed = 4.4f;
        [SerializeField] private float depthLateralSpeed = 3.2f;
        [SerializeField] private Vector3 depthForward = Vector3.forward;
        [SerializeField] private float depthCorridorCenterX = 2.2f;
        [SerializeField] private float depthCorridorHalfWidth = 2.3f;
        [SerializeField] private bool constrainDepthTraversal;
        [SerializeField] private Camera movementCamera;

        [Header("Shared movement")]
        [SerializeField] private float acceleration = 28f;
        [SerializeField, Range(0f, 1f)] private float airControl = 0.72f;
        [SerializeField] private float jumpHeight = 1.4f;
        [SerializeField] private float gravity = -32f;
        [SerializeField] private PlayerMovementMode movementMode = PlayerMovementMode.SideScroll;

        [Header("Reliability")]
        [SerializeField] private float fallResetY = -12f;
        [SerializeField] private Transform visualRoot;

        [Header("Development")]
        [SerializeField] private bool showMovementModeIndicator = true;

        private CharacterController controller;
        private Vector3 planarVelocity;
        private float verticalVelocity;
        private PlayerMovementMode resumeMode = PlayerMovementMode.SideScroll;

        private Vector3 checkpointPosition;
        private PlayerMovementMode checkpointMode;
        private PerceptionState checkpointPerception;
        private DepthJourneyCamera checkpointJourney;
        private float checkpointLaneZ;
        private Vector3 checkpointDepthForward;
        private float checkpointCorridorCenterX;
        private float checkpointCorridorHalfWidth;
        private GUIStyle movementModeStyle;

        public PlayerMovementMode CurrentMode => movementMode;
        public event Action<PlayerJumpStyle> Jumped;
        public float NormalizedSpeed
        {
            get
            {
                if (movementMode == PlayerMovementMode.DepthTraversal)
                {
                    float maxDepthSpeed = Mathf.Max(depthForwardSpeed, depthLateralSpeed);
                    Vector3 groundVelocity = Vector3.ProjectOnPlane(planarVelocity, Vector3.up);
                    return maxDepthSpeed <= 0.01f
                        ? 0f
                        : Mathf.Clamp01(groundVelocity.magnitude / maxDepthSpeed);
                }

                return sideMovementSpeed <= 0.01f
                    ? 0f
                    : Mathf.Clamp01(Mathf.Abs(planarVelocity.x) / sideMovementSpeed);
            }
        }
        public Vector3 PlanarVelocity => Vector3.ProjectOnPlane(planarVelocity, Vector3.up);
        public float VerticalVelocity => verticalVelocity;
        public bool IsGrounded => controller != null && controller.isGrounded;
        public Vector3 MovementAxis => movementMode == PlayerMovementMode.DepthTraversal
            ? depthForward
            : Vector3.right;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            checkpointPosition = transform.position;
            checkpointMode = PlayerMovementMode.SideScroll;
            checkpointPerception = PerceptionState.Reality;
            checkpointJourney = DepthJourneyCamera.ValleyPassage;
            checkpointLaneZ = sideLaneZ;
            checkpointDepthForward = depthForward;
            checkpointCorridorCenterX = depthCorridorCenterX;
            checkpointCorridorHalfWidth = depthCorridorHalfWidth;
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            float horizontalInput = 0f;
            float forwardInput = 0f;
            bool jumpPressed = false;

            if (movementMode != PlayerMovementMode.ControlLocked && keyboard != null)
            {
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                    horizontalInput += 1f;
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                    horizontalInput -= 1f;

                if (movementMode == PlayerMovementMode.DepthTraversal)
                {
                    if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                        forwardInput += 1f;
                    if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                        forwardInput -= 1f;
                }

                jumpPressed = keyboard.spaceKey.wasPressedThisFrame;
            }

            bool grounded = controller.isGrounded;
            float control = grounded ? 1f : airControl;
            Vector3 desired = movementMode == PlayerMovementMode.DepthTraversal
                ? GetDepthTraversalVelocity(horizontalInput, forwardInput)
                : GetSideScrollVelocity(horizontalInput);

            if (movementMode == PlayerMovementMode.ControlLocked)
                desired = Vector3.zero;

            planarVelocity = Vector3.MoveTowards(
                planarVelocity, desired, acceleration * control * Time.deltaTime);

            if (grounded && verticalVelocity < 0f)
                verticalVelocity = -2f;
            if (grounded && jumpPressed)
            {
                PlayerJumpStyle jumpStyle = GetJumpStyleAtTakeoff();
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                Jumped?.Invoke(jumpStyle);
            }

            verticalVelocity += gravity * Time.deltaTime;
            controller.Move((planarVelocity + Vector3.up * verticalVelocity) * Time.deltaTime);

            if (movementMode == PlayerMovementMode.DepthTraversal && constrainDepthTraversal)
                ConstrainToDepthCorridor();

            FaceMovementDirection();

            if (transform.position.y < fallResetY)
                RestoreCheckpointMovementAndPosition();
        }

        private Vector3 GetSideScrollVelocity(float horizontalInput)
        {
            float laneError = sideLaneZ - transform.position.z;
            float laneCorrection = Mathf.Clamp(
                laneError * sideLaneCorrectionSpeed,
                -sideMovementSpeed,
                sideMovementSpeed);
            return Vector3.right * (horizontalInput * sideMovementSpeed)
                + Vector3.forward * laneCorrection;
        }

        private Vector3 GetDepthTraversalVelocity(float lateralInput, float forwardInput)
        {
            Vector2 input = Vector2.ClampMagnitude(
                new Vector2(lateralInput, forwardInput),
                1f);
            GetCameraGroundBasis(out Vector3 forward, out Vector3 right);
            return forward * (input.y * depthForwardSpeed)
                + right * (input.x * depthLateralSpeed);
        }

        private void GetCameraGroundBasis(out Vector3 forward, out Vector3 right)
        {
            if (movementCamera == null)
                movementCamera = Camera.main;

            Vector3 sourceForward = movementCamera != null
                ? movementCamera.transform.forward
                : depthForward;
            Vector3 sourceRight = movementCamera != null
                ? movementCamera.transform.right
                : Vector3.Cross(Vector3.up, depthForward);

            forward = Vector3.ProjectOnPlane(sourceForward, Vector3.up);
            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.ProjectOnPlane(depthForward, Vector3.up);
            forward.Normalize();

            right = Vector3.ProjectOnPlane(sourceRight, Vector3.up);
            right -= forward * Vector3.Dot(right, forward);
            if (right.sqrMagnitude < 0.001f)
                right = Vector3.Cross(Vector3.up, forward);
            right.Normalize();
        }

        private void ConstrainToDepthCorridor()
        {
            Vector3 forward = Vector3.ProjectOnPlane(depthForward, Vector3.up).normalized;
            Vector3 lateral = Vector3.Cross(Vector3.up, forward).normalized;
            float offset = Vector3.Dot(transform.position, lateral) - depthCorridorCenterX;
            float clamped = Mathf.Clamp(offset, -depthCorridorHalfWidth, depthCorridorHalfWidth);
            float correction = clamped - offset;
            if (Mathf.Abs(correction) > 0.001f)
                controller.Move(lateral * correction);
        }

        private void FaceMovementDirection()
        {
            Vector3 groundDirection = Vector3.ProjectOnPlane(planarVelocity, Vector3.up);
            if (visualRoot == null || groundDirection.sqrMagnitude <= 0.04f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(groundDirection.normalized, Vector3.up);
            visualRoot.rotation = Quaternion.Slerp(
                visualRoot.rotation, targetRotation, 12f * Time.deltaTime);
        }

        private PlayerJumpStyle GetJumpStyleAtTakeoff()
        {
            float speed = NormalizedSpeed;
            if (speed < 0.12f)
                return PlayerJumpStyle.Stationary;
            return speed < 0.82f
                ? PlayerJumpStyle.Walking
                : PlayerJumpStyle.Running;
        }

        public void SetSideScrollMode(float laneZ)
        {
            sideLaneZ = laneZ;
            movementMode = PlayerMovementMode.SideScroll;
            resumeMode = PlayerMovementMode.SideScroll;
        }

        public void SetDepthTraversalMode(Vector3 forward, float corridorCenterX, float corridorHalfWidth)
        {
            if (forward.sqrMagnitude > 0.001f)
                depthForward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
            depthCorridorCenterX = corridorCenterX;
            depthCorridorHalfWidth = Mathf.Max(0.5f, corridorHalfWidth);
            movementMode = PlayerMovementMode.DepthTraversal;
            resumeMode = PlayerMovementMode.DepthTraversal;
        }

        public void SetControlLocked(bool locked)
        {
            if (locked)
            {
                if (movementMode != PlayerMovementMode.ControlLocked)
                    resumeMode = movementMode;
                movementMode = PlayerMovementMode.ControlLocked;
                planarVelocity = Vector3.zero;
            }
            else
            {
                movementMode = resumeMode;
            }
        }

        public void SetCheckpoint(
            Vector3 position,
            PerceptionState perception,
            PlayerMovementMode mode,
            DepthJourneyCamera journey = DepthJourneyCamera.ValleyPassage)
        {
            checkpointPosition = position;
            checkpointPerception = perception;
            checkpointMode = mode;
            checkpointJourney = journey;
            checkpointLaneZ = sideLaneZ;
            checkpointDepthForward = depthForward;
            checkpointCorridorCenterX = depthCorridorCenterX;
            checkpointCorridorHalfWidth = depthCorridorHalfWidth;
        }

        public void RecoverToCheckpoint(DepthCameraStateController cameraState)
        {
            RestoreCheckpointMovementAndPosition();
            if (cameraState != null)
                cameraState.RestorePresentation(checkpointPerception, checkpointMode, checkpointJourney, 0.45f);
        }

        public void Teleport(Vector3 position)
        {
            bool wasEnabled = controller.enabled;
            controller.enabled = false;
            transform.position = position;
            planarVelocity = Vector3.zero;
            verticalVelocity = 0f;
            controller.enabled = wasEnabled;
        }

        private void RestoreCheckpointMovementAndPosition()
        {
            Teleport(checkpointPosition);
            sideLaneZ = checkpointLaneZ;
            depthForward = checkpointDepthForward;
            depthCorridorCenterX = checkpointCorridorCenterX;
            depthCorridorHalfWidth = checkpointCorridorHalfWidth;
            movementMode = checkpointMode;
            resumeMode = checkpointMode;
        }

        private void OnGUI()
        {
            if (!showMovementModeIndicator)
                return;

            string label = movementMode == PlayerMovementMode.ControlLocked
                ? "LOCKED"
                : movementMode == PlayerMovementMode.DepthTraversal
                    ? "DEPTH TRAVERSAL"
                    : "SIDE SCROLL";

            if (movementModeStyle == null)
            {
                movementModeStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 12,
                    fontStyle = FontStyle.Bold
                };
                movementModeStyle.normal.textColor = new Color(0.51f, 0.91f, 0.93f, 1f);
            }

            Rect indicator = new Rect(14f, 14f, 154f, 26f);
            Color previousColor = GUI.color;
            GUI.color = new Color(0.027f, 0.035f, 0.075f, 0.78f);
            GUI.DrawTexture(indicator, Texture2D.whiteTexture);
            GUI.color = previousColor;
            GUI.Label(indicator, label, movementModeStyle);
        }
    }
}
