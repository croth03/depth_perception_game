using UnityEngine;

namespace DepthPerception
{
    /// <summary>Plays the existing in-place humanoid walk only while the authored Figure sequence moves its root.</summary>
    public sealed class FigureApproachWalkDriver : MonoBehaviour
    {
        [SerializeField] private Animator figureAnimator;
        [SerializeField, Min(0.01f)] private float movementThreshold = 0.08f;

        private Vector3 previousPosition;
        private bool walking;

        private void Awake()
        {
            if (figureAnimator == null)
                figureAnimator = GetComponentInChildren<Animator>(true);
            if (figureAnimator != null)
            {
                figureAnimator.applyRootMotion = false;
                figureAnimator.speed = 0f;
            }
            previousPosition = transform.position;
        }

        private void LateUpdate()
        {
            if (figureAnimator == null)
                return;

            float speed = Vector3.Distance(transform.position, previousPosition) / Mathf.Max(Time.deltaTime, 0.0001f);
            previousPosition = transform.position;
            bool shouldWalk = speed >= movementThreshold;
            if (shouldWalk == walking)
                return;

            walking = shouldWalk;
            figureAnimator.speed = walking ? 1f : 0f;
        }
    }
}
