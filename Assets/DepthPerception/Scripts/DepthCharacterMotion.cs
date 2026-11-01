using UnityEngine;

namespace DepthPerception
{
    public sealed class DepthCharacterMotion : MonoBehaviour
    {
        [SerializeField] private DepthPlayerController controller;
        [SerializeField] private Transform visualRoot;
        [SerializeField] private Transform body;
        [SerializeField] private Transform head;
        [SerializeField] private Transform leftLeg;
        [SerializeField] private Transform rightLeg;

        [Header("Motion")]
        [SerializeField] private float gaitFrequency = 2.5f;
        [SerializeField] private float gaitBob = 0.08f;
        [SerializeField] private float gaitRoll = 3f;
        [SerializeField] private float legSwing = 24f;
        [SerializeField] private float idleBreath = 0.025f;

        private Vector3 rootBasePosition;
        private Quaternion bodyBaseRotation;
        private Quaternion headBaseRotation;
        private Quaternion leftBaseRotation;
        private Quaternion rightBaseRotation;

        private void Awake()
        {
            rootBasePosition = visualRoot.localPosition;
            bodyBaseRotation = body.localRotation;
            headBaseRotation = head.localRotation;
            leftBaseRotation = leftLeg.localRotation;
            rightBaseRotation = rightLeg.localRotation;
        }

        private void LateUpdate()
        {
            float speed = controller.NormalizedSpeed;
            float gait = Mathf.Sin(Time.time * gaitFrequency * Mathf.PI * 2f);
            float breathe = Mathf.Sin(Time.time * 0.9f * Mathf.PI * 2f);

            float bob = speed > 0.03f ? gait * gaitBob * speed : breathe * idleBreath;
            visualRoot.localPosition = rootBasePosition + Vector3.up * bob;

            float roll = gait * gaitRoll * speed;
            body.localRotation = bodyBaseRotation * Quaternion.Euler(0f, 0f, roll);

            float swing = gait * legSwing * speed;
            leftLeg.localRotation = leftBaseRotation * Quaternion.Euler(swing, 0f, 0f);
            rightLeg.localRotation = rightBaseRotation * Quaternion.Euler(-swing, 0f, 0f);

            head.localRotation = headBaseRotation
                * Quaternion.Euler(0f, breathe * 3.5f, -roll * 0.25f);
        }
    }
}
