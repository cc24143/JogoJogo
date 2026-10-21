
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    private bool EmMovimento = false;
    private float velocidade = 2.0f;

    void Update()
    {
        if (EmMovimento)
        {
            transform.Translate(
                Vector3.right * velocidade * Time.deltaTime
            );
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EmMovimento = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EmMovimento = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EmMovimento = false;
        }
    }
}
