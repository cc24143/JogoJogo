using UnityEngine;

public class GravityOrb : MonoBehaviour
{
    public float cooldown = 0.5f; // evita ativar várias vezes seguidas

    private float lastUseTime = -999f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null) return;
        if (other.gameObject != other.attachedRigidbody.gameObject) return; // ignora colliders filhos
        var player = other.attachedRigidbody.GetComponent<Player>();
        if (player != null) player.SetCurrentOrb(this);
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null) player.ClearCurrentOrb(this);
    }

    // Chamado pelo Player quando ele aperta pular dentro do orb
    public void Activate(ObjectGravityController controller)
    {
        if (controller == null) return;
        if (Time.time - lastUseTime < cooldown) return;

        lastUseTime = Time.time;
        controller.ToggleGravity();
    }
}