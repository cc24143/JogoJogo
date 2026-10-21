using UnityEngine;

public class Crate : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private BoxMovement boxMovement;
    private BoxCollider boxCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxMovement = GetComponent<BoxMovement>();
        boxCollider = GetComponent<BoxCollider>();
        rb.isKinematic = false; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            rb.isKinematic = true;
        }
    }

    public void Interagir(Vector3 direction)
    {
        if (boxMovement.IsMoving()) return;

        // 1. Pegamos as dimensões reais da caixa
        Vector3 tamanhoDoCubo = boxCollider.bounds.size;
        
        // Ajuste Fino: Mantemos as laterais quase idênticas à caixa (1.02f) para não pegar o player do lado
        tamanhoDoCubo.x *= 0.45f;
        tamanhoDoCubo.z *= 0.45f;
        tamanhoDoCubo.y = 0.2f; // Uma camada fina bem no topo

        // 2. Subimos o centro do sensor um pouco mais (0.25f em vez de 0.15f)
        // Isso cria uma "folga" vertical para o sensor flutuar logo acima do teto, ignorando o player do lado
        Vector3 centroDoSensor = transform.position + new Vector3(0, boxCollider.bounds.extents.y + 0.5f, 0);

        // 3. Filtro da camada do Player
        int layerDoPlayer = LayerMask.GetMask("Player");

        // 4. Varredura
        Collider[] collidersNoTopo = Physics.OverlapBox(centroDoSensor, tamanhoDoCubo / 2, transform.rotation, layerDoPlayer);

        if (collidersNoTopo.Length > 0)
        {
            Debug.Log("Não pode empurrar: O Player está em cima da caixa!");
            return; 
        }

        // Se o topo estiver limpo, move a caixa
        rb.isKinematic = true;
        boxMovement.ChangeMovementStatus(direction);
    }

}
