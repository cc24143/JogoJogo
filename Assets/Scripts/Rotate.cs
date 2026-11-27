using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float velocidade = 50f;

    void Update()
    {
        transform.Rotate(0f, 0f, -velocidade * Time.deltaTime);
    }
}