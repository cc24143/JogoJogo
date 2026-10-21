using UnityEngine;

public class Crate : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private BoxMovement boxMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxMovement = GetComponent<BoxMovement>();
        
        // Começa caindo com a física normal
        rb.isKinematic = false; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Se tocou o chão, ativa o Kinematic para travar a física pesada
        if (collision.gameObject.CompareTag("Floor"))
        {
            rb.isKinematic = true;
        }
    }

    public void Interagir(Vector3 direction)
    {
        // Garante que está Kinematic para o transform.Translate do BoxMovement mover a caixa livremente
        rb.isKinematic = true;
        
        boxMovement.ChangeMovementStatus(direction);
    } 
}
