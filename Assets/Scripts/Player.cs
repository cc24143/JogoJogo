using System;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private WalkingInput playerMovement;
    private BoxCollider boxCollider;

    private bool isTryingJump;
    private bool isGrounded;

    private Vector2 forceDirection = new();
    
    public Transform groundCheck;
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
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, playerMovement.groundDistance, groundMask);
        }
    }

    void FixedUpdate()
    {
        performMovement();
        if(isTryingJump && isGrounded)
        {
            performJump();
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
