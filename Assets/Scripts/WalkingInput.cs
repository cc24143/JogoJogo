using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class WalkingInput : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float baseForce = 10f;

    private Vector2 forceDirection = new();
    private Rigidbody playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
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
}
