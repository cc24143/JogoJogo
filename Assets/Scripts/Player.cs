using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private WalkingInput playerMovement;
    private BoxCollider boxCollider;

    private bool isTryingJump;
    private bool isGrounded;
    private bool groundedThisFrame;

    private Vector2 forceDirection = new();
    
    public LayerMask groundMask;
    private Transform cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        playerMovement = GetComponent<WalkingInput>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        isGrounded = groundedThisFrame;
        groundedThisFrame = false;

        performMovement();
        if (isTryingJump && isGrounded)
            performJump();
    }

    void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundMask) == 0) return;

        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                groundedThisFrame = true;
                return;
            }
        }
    }

    public void ApplyMovement(InputAction.CallbackContext ctx)
    {
        forceDirection = ctx.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        isTryingJump = ctx.performed;
    }

    public void performMovement()
    {
        playerMovement.ApplyMovement(rb, forceDirection, cam);
    }

    public void performJump()
    {
        playerMovement.Jump(rb);
    }
}
