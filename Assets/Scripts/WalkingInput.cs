using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class WalkingInput : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float baseForce = 10f;

    public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private Vector2 forceDirection = new();
    private Rigidbody playerRigidbody;
    private bool isGrounded;

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
            playerRigidbody.AddForce(forceDirection.normalized.x * baseForce, 0, 0);
        if (Math.Abs(playerRigidbody.linearVelocity.z) <= maxSpeed)
            playerRigidbody.AddForce(0, 0, forceDirection.normalized.y * baseForce);
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
        if (ctx.performed && isGrounded)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
