using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour, IInteractable
{
    [Header("Evento disparado ao apertar o botão")]
    public UnityEvent aoApertar;

    [Header("Opções")]
    [SerializeField] private bool apenasUmaVez = false;
    [SerializeField] private float cooldown = 0.3f;

    private bool jaUsado;
    private float ultimoUso = -999f;

    // Necessário porque a interface declara "Transform transform { get; }".
    // O MonoBehaviour já tem "transform", então isso é satisfeito automaticamente.

    public void Interagir(Vector3 direction)
    {
        if (apenasUmaVez && jaUsado) return;
        if (Time.time - ultimoUso < cooldown) return;

        ultimoUso = Time.time;
        jaUsado = true;

        aoApertar?.Invoke();
    }
}