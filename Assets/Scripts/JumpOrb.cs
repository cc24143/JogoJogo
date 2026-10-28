using UnityEngine;

public class JumpOrb : MonoBehaviour, IInteractable
{
    public Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interagir(Vector3 direction)
    {
        player.performJump(true);
    }
}
