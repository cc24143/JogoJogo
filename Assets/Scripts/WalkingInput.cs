using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ObjectGravityController))]
public class WalkingInput : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float walkForce = 10f;
    public float jumpForce = 5f;
    public float groundDistance = 0.2f;

    [Header("Rotação")]
    public Transform visual;            // opcional: se vazio, gira o próprio Robo
    public float turnSpeed = 720f;      // graus por segundo
    public float modelYawOffset = 180f; // 0, 90, -90 ou 180 conforme o rosto

    private ObjectGravityController gravity;
    private Vector3 lastMoveDir = Vector3.forward;
    private Transform rotTarget;
    private bool hasDirection = false;

    public bool HasMoveInput { get; private set; }

    void Awake()
    {
        gravity = GetComponent<ObjectGravityController>();
        rotTarget = visual != null ? visual : transform;

        if (visual == null)
            Debug.LogWarning("WalkingInput: campo 'Visual' vazio, girando o objeto raiz.", this);
    }

    void Update()
    {
        // Sem input ainda: não mexe na rotação, só corrige o "cima" depois
        Vector3 up = gravity.IsGravityInverted() ? Vector3.down : Vector3.up;

        Vector3 forward = hasDirection ? lastMoveDir : GetCurrentForward();

        Quaternion target = Quaternion.LookRotation(forward, up)
            * Quaternion.Euler(0f, modelYawOffset, 0f);

        rotTarget.rotation = Quaternion.RotateTowards(
            rotTarget.rotation, target, turnSpeed * Time.deltaTime);
    }

    // Direção horizontal para onde o robô está olhando agora
    Vector3 GetCurrentForward()
    {
        Vector3 f = rotTarget.rotation * Quaternion.Euler(0f, -modelYawOffset, 0f) * Vector3.forward;
        f.y = 0f;
        return f.sqrMagnitude > 0.0001f ? f.normalized : Vector3.forward;
    }

    public void ApplyMovement(Rigidbody playerRigidbody, Vector2 input, Transform cameraTransform)
    {
        HasMoveInput = input.sqrMagnitude >= 0.0001f;
        if (!HasMoveInput) return;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * input.y + camRight * input.x).normalized;

        lastMoveDir = moveDir;
        hasDirection = true;

        Vector3 horizontalVel = playerRigidbody.linearVelocity;
        horizontalVel.y = 0f;

        if (horizontalVel.magnitude < maxSpeed)
            playerRigidbody.AddForce(moveDir * walkForce, ForceMode.Acceleration);
    }

    public void ClearMoveInput()
    {
        HasMoveInput = false;
    }

    public void Jump(Rigidbody playerRigidbody, bool resetVertical = false)
    {
        if (resetVertical)
        {
            Vector3 vel = playerRigidbody.linearVelocity;
            vel.y = 0f;
            playerRigidbody.linearVelocity = vel;
        }
        float direction = gravity.IsGravityInverted() ? -1f : 1f;
        playerRigidbody.AddForce(0, jumpForce * direction, 0, ForceMode.Impulse);
    }
}