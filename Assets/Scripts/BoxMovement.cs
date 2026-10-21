using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    private bool inMovement = false;
    private float velocity = 5.0f;
    private Vector3 direction = Vector3.right;

    void Update()
    {
        if (inMovement)
        {
            transform.Translate(direction * velocity * Time.deltaTime, Space.World);
        }
    }

    // Usando OnTriggerEnter para detectar a área das calhas (Gutter)
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

    private void CheckAndStop(Collider collider)
    {
        // Encontra o ponto mais próximo no collider da calha em relação à caixa
        Vector3 pontoMaisProximo = collider.ClosestPoint(transform.position);
        Vector3 direcaoParaParede = (pontoMaisProximo - transform.position).normalized;

        // Se o vetor de movimento atual apontar na direção desse ponto, nós paramos
        if (Vector3.Dot(direction, direcaoParaParede) > 0.1f)
        {
            Debug.Log("Stop");
            inMovement = false;
        }
    }

    public void ChangeMovementStatus(Vector3 newDirection)
    {
        if (inMovement == false)
        {
            direction = newDirection;
            inMovement = true;
        }
    }
}
