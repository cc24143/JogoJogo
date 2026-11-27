using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float height = 5.0f;

    public float rotationDamping = 10f;
    public float heightDamping = 10f;
    public float rotationSensitivity = 1f;

    [Header("Colisão")]
    public LayerMask obstacleMask;
    public float collisionMargin = 0.3f;
    public float collisionRadius = 0.25f;
    public float minDistance = 1.5f;
    public float pivotHeight = 1.5f;
    public float distanceRecoverSpeed = 4f;

    private float mouseRotationAxis = 0;
    private float currentRotationAngle;
    private float currentHeight;
    private float currentDistance;
    private bool isInverted = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentRotationAngle = transform.eulerAngles.y;
        currentHeight = transform.position.y;
        currentDistance = Mathf.Sqrt(distance * distance + height * height);
    }

    public void SetInverted(bool inverted) => isInverted = inverted;

    void LateUpdate()
    {
        if (!target) return;

        float sign = isInverted ? -1f : 1f;

        float wantedRotationAngle = mouseRotationAxis * rotationSensitivity;
        float wantedHeight = target.position.y + height * sign;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        Vector3 desiredPos = target.position - currentRotation * Vector3.forward * distance;
        desiredPos.y = currentHeight;

        Vector3 origin = target.position + Vector3.up * (pivotHeight * sign);
        Vector3 dir = desiredPos - origin;
        float fullDist = dir.magnitude;
        Vector3 dirN = dir / fullDist;

        float allowedDist = fullDist;
        if (Physics.SphereCast(origin, collisionRadius, dirN, out RaycastHit hit, fullDist,
                               obstacleMask, QueryTriggerInteraction.Ignore))
        {
            allowedDist = Mathf.Max(hit.distance - collisionMargin, minDistance);
        }

        if (allowedDist < currentDistance)
            currentDistance = allowedDist;
        else
            currentDistance = Mathf.Lerp(currentDistance, allowedDist, distanceRecoverSpeed * Time.deltaTime);

        currentDistance = Mathf.Min(currentDistance, fullDist);

        transform.position = origin + dirN * currentDistance;

        Vector3 up = isInverted ? Vector3.down : Vector3.up;
        transform.LookAt(target.position + Vector3.up * (pivotHeight * 0.5f * sign), up);
    }

    public void ChangeRotationMouse(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        float delta = ctx.ReadValue<float>();
        mouseRotationAxis += isInverted ? -delta : delta;
    }
}