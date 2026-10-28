using UnityEngine;

public class GravityOrb : MonoBehaviour
{
    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        Toggle(other.GetComponentInParent<ObjectGravityController>());
    }

    private void OnCollisionEnter(Collision collision)
    {
        Toggle(collision.gameObject.GetComponentInParent<ObjectGravityController>());
    }

    private void Toggle(ObjectGravityController controller)
    {
        if (used || controller == null) return;
        used = true;
        controller.ToggleGravity();
        Destroy(gameObject);
    }
}