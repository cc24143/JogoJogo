using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class WalkingInput : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float walkForce = 10f;
    public float jumpForce = 5f;

    public float groundDistance = 0.2f;

    public Transform groundCheck;
    public LayerMask groundMask;

    private bool isTryingJump;
    private bool isGrounded;

    private Vector2 forceDirection = new();
    private Rigidbody playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
    }

    void FixedUpdate()
    {
        if (Math.Abs(playerRigidbody.linearVelocity.x) <= maxSpeed)
            playerRigidbody.AddForce(forceDirection.normalized.x * walkForce, 0, 0);
        if (Math.Abs(playerRigidbody.linearVelocity.z) <= maxSpeed)
            playerRigidbody.AddForce(0, 0, forceDirection.normalized.y * walkForce);

        if (isTryingJump && isGrounded)
            playerRigidbody.AddForce(0, jumpForce, 0);
    }

    public void ApplyMovement(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            forceDirection = ctx.ReadValue<Vector2>();
        }
        else
        {
            forceDirection.Set(0, 0);
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isTryingJump = true;
        }
        else
        {
            isTryingJump = false;
        }
    }
}
