using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    private bool inMovement = false;
    private float velocity = 5.0f;
    private Vector3 direction = Vector3.right;
    
    // Guarda a direção bloqueada pela parede para impedir novos cliques nela
    private Vector3 direcaoBloqueada = Vector3.zero;

    void Update()
    {
        if (inMovement)
        {
            transform.Translate(direction * velocity * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Gutter"))
        {
            CheckAndStop(collider);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Gutter"))
        {
            CheckAndStop(collider);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Gutter"))
        {
            // Quando se afastar da parede, libera o movimento naquela direção novamente
            direcaoBloqueada = Vector3.zero;
        }
    }

    private void CheckAndStop(Collider collider)
    {
        Vector3 pontoMaisProximo = collider.ClosestPoint(transform.position);
        Vector3 direcaoParaParede = (pontoMaisProximo - transform.position).normalized;

        if (Vector3.Dot(direction, direcaoParaParede) > 0.1f)
        {
            inMovement = false;
            // Salva a direção atual como bloqueada para evitar que novos cliques forcem a entrada na parede
            direcaoBloqueada = direction; 
        }
    }

    public void ChangeMovementStatus(Vector3 newDirection)
    {
        // Só aceita o movimento se não estiver andando E se a nova direção não for a direção bloqueada
        if (!inMovement && newDirection != direcaoBloqueada)
        {
            direction = newDirection;
            inMovement = true;
        }
    }

    public bool IsMoving()
    {
        return inMovement;
    }
}
