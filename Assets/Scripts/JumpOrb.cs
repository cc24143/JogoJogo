using UnityEngine;

public class JumpOrb : MonoBehaviour, IInteractable
{
    public Player player;

    public void Interagir(Vector3 direction)
    {
        player.performJump(true);
    }
}
