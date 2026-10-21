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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Gutter"))
        {
            CheckAndStop(collision);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Gutter"))
        {
            CheckAndStop(collision);
        }
    }

    private void CheckAndStop(Collision collision)
    {
        // Pega a normal do primeiro ponto de colisão (aponta para fora da parede)
        Vector3 wallNormal = collision.contacts[0].normal;

        // Se a direção do movimento for oposta à normal da parede (ou seja, tentando entrar na parede)
        if (Vector3.Dot(direction, wallNormal) < -0.5f)
        {
            inMovement = false;
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    public void ChangeMovementStatus()
    {
        inMovement = true;
    }
}