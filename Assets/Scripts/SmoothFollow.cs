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

    private float mouseRotationAxis = 0;
    private bool isInverted = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetInverted(bool inverted) => isInverted = inverted;

    void LateUpdate()
    {
        if (!target) return;

        float sign = isInverted ? -1f : 1f;

        var wantedRotationAngle = mouseRotationAxis * rotationSensitivity;
        var wantedHeight = target.position.y + height * sign;

        var currentRotationAngle = transform.eulerAngles.y;
        var currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        Vector3 desiredPos = target.position - currentRotation * Vector3.forward * distance;
        desiredPos.y = currentHeight;

        Vector3 dir = desiredPos - target.position;
        float dist = dir.magnitude;

        if (dist > 0.001f &&
            Physics.Raycast(target.position, dir / dist, out RaycastHit hit, dist, obstacleMask, QueryTriggerInteraction.Ignore))
        {
            desiredPos = hit.point - (dir / dist) * collisionMargin;
        }

        transform.position = desiredPos;

        // Suaviza a virada: "cima" da câmera acompanha a altura atual
        Vector3 up = isInverted ? Vector3.down : Vector3.up;
        transform.LookAt(target, up);
    }

    public void ChangeRotationMouse(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            mouseRotationAxis += ctx.ReadValue<float>();
    }
}