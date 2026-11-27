using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask groundMask;

    private Rigidbody rb;
    private WalkingInput playerMovement;
    private ObjectGravityController gravity;
    private Transform cam;
    private SmoothFollow camFollow;
    private InteractionInput interactionInput;

    private bool isTryingJump;
    private bool isGrounded;
    public bool IsGrounded => isGrounded;
    private bool groundedThisStep;
    private bool cameraInverted = false;
    private Vector2 forceDirection = new();

    private GravityOrb currentOrb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<WalkingInput>();
        gravity = GetComponent<ObjectGravityController>();
        cam = Camera.main.transform;
        camFollow = cam.GetComponent<SmoothFollow>();
        interactionInput = GetComponentInChildren<InteractionInput>();
        rb.sleepThreshold = 0f;
    }

    void Update()
    {
        if (cameraInverted != gravity.IsGravityInverted())
        {
            cameraInverted = gravity.IsGravityInverted();
            if (camFollow != null) camFollow.SetInverted(cameraInverted);
        }
    }

    void FixedUpdate()
    {
        isGrounded = groundedThisStep;
        groundedThisStep = false;

        rb.angularVelocity = Vector3.zero;

        performMovement();
        if (isTryingJump && isGrounded)
        {
            performJump();
        }
        isTryingJump = false;
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

        if (currentOrb != null)
        {
            currentOrb.Activate(gravity);
            return;
        }
        else if (interactionInput != null && interactionInput.TentarUsarOrb())
        {
            isTryingJump = false;
            return;
        }
        isTryingJump = true;
    }

    void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundMask) == 0) return;

        float up = gravity.IsGravityInverted() ? -1f : 1f;

        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y * up > 0.5f)
            {
                groundedThisStep = true;
                return;
            }
        }
    }

    public void performMovement()
    {
        playerMovement.ApplyMovement(rb, forceDirection, cam);
    }

    public void performJump(bool resetVertical = false)
    {
        playerMovement.Jump(rb, resetVertical);
    }
}