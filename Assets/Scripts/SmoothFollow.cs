using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float distance = 10.0f;
	public float height = 5.0f;

	public float rotationDamping;
	public float heightDamping;
	public float rotationSensitivity;

	private float mouseRotationAxis = 0;
	private Camera cam;

    void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		cam = GetComponent<Camera>();
    }

    void LateUpdate()
	{
		if (!target)
		{
			return;
		}

        var ray = new Ray(target.transform.position, cam.transform.forward);
		float hitDistance;

        if (Physics.Raycast(ray, out RaycastHit hit, distance)) {
            Transform objectHit = hit.transform;
            
			hitDistance = Vector2.Distance(cam.transform.position, objectHit.position);
        }
		else
		{
			hitDistance = height;
		}

		var wantedRotationAngle = target.eulerAngles.y + mouseRotationAxis * rotationSensitivity;
		var wantedHeight = target.position.y + hitDistance;

		var currentRotationAngle = transform.eulerAngles.y;
		var currentHeight = transform.position.y;

		currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

		transform.position = target.position;
		transform.position -= currentRotation * Vector3.forward * distance;

		transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

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