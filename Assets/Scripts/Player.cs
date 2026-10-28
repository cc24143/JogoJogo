using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Transform groundCheck;   // filho nos pés do player
    public LayerMask groundMask;

    private Rigidbody rb;
    private WalkingInput playerMovement;
    private ObjectGravityController gravity;
    private Transform cam;
    private SmoothFollow camFollow;

    private bool isTryingJump;
    private bool isGrounded;
    private bool cameraInverted = false;
    private Vector2 forceDirection = new();

    private GravityOrb currentOrb; // orb em que o player está dentro (ou null)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<WalkingInput>();
        gravity = GetComponent<ObjectGravityController>();
        cam = Camera.main.transform;
        camFollow = cam.GetComponent<SmoothFollow>();
    }

    void Update()
    {
        if (groundCheck == null) return;

        Vector3 checkPos = groundCheck.position;
        if (gravity.IsGravityInverted())
            checkPos = transform.position + (transform.position - groundCheck.position);

        isGrounded = Physics.CheckSphere(checkPos, playerMovement.groundDistance, groundMask, QueryTriggerInteraction.Ignore);

        // A câmera só acompanha a gravidade quando o player toca a nova superfície
        if (isGrounded && cameraInverted != gravity.IsGravityInverted())
        {
            cameraInverted = gravity.IsGravityInverted();
            if (camFollow != null) camFollow.SetInverted(cameraInverted);
        }
    }

    void FixedUpdate()
    {
        playerMovement.ApplyMovement(rb, forceDirection, cam);

        if (isTryingJump && isGrounded)
        {
            playerMovement.Jump(rb, gravity.IsGravityInverted() ? -1f : 1f);
            isTryingJump = false;
        }
    }

    public void SetCurrentOrb(GravityOrb orb)
    {
        currentOrb = orb;
    }

    public void ClearCurrentOrb(GravityOrb orb)
    {
        if (currentOrb == orb) currentOrb = null;
    }

    public void ApplyMovement(InputAction.CallbackContext ctx)
    {
        forceDirection = ctx.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        // Dentro do orb, o botão de pular ativa a gravidade e não pula
        if (currentOrb != null)
        {
            currentOrb.Activate(gravity);
            return;
        }

        isTryingJump = true;
    }
}