using UnityEngine;

public sealed class CodexRotationTest : MonoBehaviour
{
    private const float RotationSpeedDegreesPerSecond = 45f;

    private void Update()
    {
        transform.Rotate(Vector3.up, RotationSpeedDegreesPerSecond * Time.deltaTime, Space.World);
    }
}
