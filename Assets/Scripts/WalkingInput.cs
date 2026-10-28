using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WalkingInput : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float walkForce = 10f;
    public float jumpForce = 5f;
    public float groundDistance = 0.2f;

    public void ApplyMovement(Rigidbody playerRigidbody, Vector2 input, Transform cameraTransform)
    {
        if (input.sqrMagnitude < 0.0001f) return;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * input.y + camRight * input.x).normalized;

        Vector3 horizontalVel = playerRigidbody.linearVelocity;
        horizontalVel.y = 0f;

        if (horizontalVel.magnitude < maxSpeed)
            playerRigidbody.AddForce(moveDir * walkForce, ForceMode.Acceleration);
    }

    // direction: 1 = para cima (normal), -1 = para baixo (gravidade invertida)
    public void Jump(Rigidbody playerRigidbody, float direction)
    {
        playerRigidbody.AddForce(0, jumpForce * direction, 0, ForceMode.Impulse);
    }
}