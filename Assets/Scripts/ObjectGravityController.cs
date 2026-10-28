using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectGravityController : MonoBehaviour
{
    public SmoothFollow cameraFollow; // opcional: se vazio, procura sozinho

    private Rigidbody rb;
    private bool isGravityInverted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        if (cameraFollow == null)
            cameraFollow = FindFirstObjectByType<SmoothFollow>();
    }

    void FixedUpdate()
    {
        Vector3 dir = isGravityInverted ? Vector3.up : Vector3.down;
        rb.AddForce(dir * Mathf.Abs(Physics.gravity.y), ForceMode.Acceleration);
    }

    public void ToggleGravity()
    {
        isGravityInverted = !isGravityInverted;
        if (cameraFollow != null) cameraFollow.SetInverted(isGravityInverted);
    }

    public bool IsGravityInverted() => isGravityInverted;
}