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
    public LayerMask obstacleMask;      // layers que bloqueiam a câmera (chão, paredes, caixas...)
    public float collisionMargin = 0.3f; // folga para a câmera não ficar colada na parede

    private float mouseRotationAxis = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!target) return;

        var wantedRotationAngle = mouseRotationAxis * rotationSensitivity;
        var wantedHeight = target.position.y + height;

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
        transform.LookAt(target);
    }

    public void ChangeRotationMouse(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            mouseRotationAxis += ctx.ReadValue<float>();
        }
    }
}