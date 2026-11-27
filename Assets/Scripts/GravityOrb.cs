using UnityEngine;

public class GravityOrb : MonoBehaviour
{
    public float cooldown = 0.5f;

    private float lastUseTime = -999f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null) return;
        if (other.gameObject != other.attachedRigidbody.gameObject) return;
        var player = other.attachedRigidbody.GetComponent<Player>();
        if (player != null) player.SetCurrentOrb(this);
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null) player.ClearCurrentOrb(this);
    }

    public void Activate(ObjectGravityController controller)
    {
        if (controller == null) return;
        if (Time.time - lastUseTime < cooldown) return;

        lastUseTime = Time.time;
        controller.ToggleGravity();
    }
}