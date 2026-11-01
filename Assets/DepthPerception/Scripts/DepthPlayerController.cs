using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DepthPerception
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class DepthPlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float movementSpeed = 5.5f;
        [SerializeField] private float acceleration = 28f;
        [SerializeField, Range(0f, 1f)] private float airControl = 0.72f;
        [SerializeField] private float jumpHeight = 1.9f;
        [SerializeField] private float gravity = -24f;
        [SerializeField] private Vector3 movementAxis = Vector3.right;

        [Header("Reliability")]
        [SerializeField] private float fallResetY = -12f;
        [SerializeField] private bool inputEnabled = true;
        [SerializeField] private Transform visualRoot;

        private CharacterController controller;
        private Vector3 horizontalVelocity;
        private float verticalVelocity;
        private Vector3 checkpoint;
        private Coroutine axisBlend;

        public float NormalizedSpeed => movementSpeed <= 0.01f
            ? 0f
            : new Vector3(horizontalVelocity.x, 0f, horizontalVelocity.z).magnitude / movementSpeed;
        public bool IsGrounded => controller != null && controller.isGrounded;
        public Vector3 MovementAxis => movementAxis;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            checkpoint = transform.position;
        }

        private void Start()
        {
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            float input = 0f;
            bool jumpPressed = false;

            if (inputEnabled && keyboard != null)
            {
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                    input += 1f;
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                    input -= 1f;
                jumpPressed = keyboard.spaceKey.wasPressedThisFrame;
            }

            bool grounded = controller.isGrounded;
            float control = grounded ? 1f : airControl;
            Vector3 desired = movementAxis.normalized * (input * movementSpeed);
            horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity, desired, acceleration * control * Time.deltaTime);

            if (grounded && verticalVelocity < 0f)
                verticalVelocity = -2f;
            if (grounded && jumpPressed)
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

            verticalVelocity += gravity * Time.deltaTime;
            controller.Move((horizontalVelocity + Vector3.up * verticalVelocity) * Time.deltaTime);

            Vector3 planar = new Vector3(horizontalVelocity.x, 0f, horizontalVelocity.z);
            if (visualRoot != null && planar.sqrMagnitude > 0.04f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(planar.normalized, Vector3.up);
                visualRoot.rotation = Quaternion.Slerp(
                    visualRoot.rotation, targetRotation, 12f * Time.deltaTime);
            }

            if (transform.position.y < fallResetY)
                Teleport(checkpoint);
        }

        public void SetInputEnabled(bool enabled)
        {
            inputEnabled = enabled;
        }

        public void SetCheckpoint(Vector3 position)
        {
            checkpoint = position;
        }

        public void SetMovementAxis(Vector3 axis)
        {
            if (axis.sqrMagnitude > 0.001f)
                movementAxis = axis.normalized;
        }

        public void BlendMovementAxis(Vector3 targetAxis, float duration)
        {
            if (axisBlend != null)
                StopCoroutine(axisBlend);
            axisBlend = StartCoroutine(BlendAxis(targetAxis.normalized, duration));
        }

        public void Teleport(Vector3 position)
        {
            bool wasEnabled = controller.enabled;
            controller.enabled = false;
            transform.position = position;
            horizontalVelocity = Vector3.zero;
            verticalVelocity = 0f;
            controller.enabled = wasEnabled;
        }

        private IEnumerator BlendAxis(Vector3 targetAxis, float duration)
        {
            Vector3 start = movementAxis.normalized;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / Mathf.Max(0.01f, duration)));
                movementAxis = Vector3.Slerp(start, targetAxis, t).normalized;
                yield return null;
            }
            movementAxis = targetAxis;
            axisBlend = null;
        }
    }
}
