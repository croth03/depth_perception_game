using UnityEngine;

namespace DepthPerception
{
    /// <summary>
    /// Feeds presentation-only locomotion values into the production player Animator.
    /// Gameplay movement remains fully owned by DepthPlayerController.
    /// </summary>
    public sealed class DepthPlayerAnimatorBridge : MonoBehaviour
    {
        [SerializeField] private DepthPlayerController controller;
        [SerializeField] private Animator animator;
        [SerializeField, Min(0f)] private float speedDamping = 0.1f;

        [Header("Grounded visual alignment")]
        [SerializeField] private bool stabilizeGroundContact = true;
        [SerializeField, Min(0f)] private float groundClearance = 0.01f;
        [SerializeField, Min(0f)] private float maxGroundCorrection = 0.25f;
        [SerializeField, Min(0f)] private float groundingSpeed = 20f;

        private static readonly int NormalizedSpeedId = Animator.StringToHash("NormalizedSpeed");
        private static readonly int GroundedId = Animator.StringToHash("Grounded");
        private static readonly int JumpId = Animator.StringToHash("Jump");

        private CharacterController characterController;
        private Renderer[] animatedRenderers;
        private float animatorBaseLocalY;
        private float groundingOffset;

        private void OnEnable()
        {
            if (controller == null)
                controller = GetComponent<DepthPlayerController>();
            if (controller != null)
                controller.Jumped += HandleJumped;
        }

        private void OnDisable()
        {
            if (controller != null)
                controller.Jumped -= HandleJumped;
        }

        private void Awake()
        {
            if (controller == null)
                controller = GetComponent<DepthPlayerController>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>(true);

            characterController = GetComponent<CharacterController>();
            if (animator != null)
            {
                animator.applyRootMotion = false;
                animator.speed = 1f;
                animatorBaseLocalY = animator.transform.localPosition.y;
                animatedRenderers = animator.GetComponentsInChildren<Renderer>(true);
            }
        }

        private void Update()
        {
            if (controller == null || animator == null)
                return;

            animator.SetFloat(
                NormalizedSpeedId,
                controller.NormalizedSpeed,
                speedDamping,
                Time.deltaTime);
            animator.SetBool(GroundedId, controller.IsGrounded);
        }

        private void HandleJumped(PlayerJumpStyle unusedJumpStyle)
        {
            if (animator == null)
                return;

            animator.ResetTrigger(JumpId);
            animator.SetTrigger(JumpId);
        }

        private void LateUpdate()
        {
            if (animator == null || characterController == null)
                return;

            float targetOffset = 0f;
            if (stabilizeGroundContact && controller.IsGrounded && TryGetAnimatedBounds(out Bounds bounds))
            {
                float desiredBottom = characterController.bounds.min.y + groundClearance;
                float correction = desiredBottom - bounds.min.y;
                targetOffset = Mathf.Clamp(
                    groundingOffset + correction,
                    -maxGroundCorrection,
                    maxGroundCorrection);
            }

            groundingOffset = Mathf.MoveTowards(
                groundingOffset,
                targetOffset,
                groundingSpeed * Time.deltaTime);

            Vector3 localPosition = animator.transform.localPosition;
            localPosition.y = animatorBaseLocalY + groundingOffset;
            animator.transform.localPosition = localPosition;
        }

        private bool TryGetAnimatedBounds(out Bounds bounds)
        {
            bounds = default;
            bool found = false;
            if (animatedRenderers == null)
                return false;

            foreach (Renderer animatedRenderer in animatedRenderers)
            {
                if (animatedRenderer == null || !animatedRenderer.enabled)
                    continue;

                if (!found)
                {
                    bounds = animatedRenderer.bounds;
                    found = true;
                }
                else
                {
                    bounds.Encapsulate(animatedRenderer.bounds);
                }
            }

            return found;
        }
    }
}
