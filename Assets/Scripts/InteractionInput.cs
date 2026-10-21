using System.Collections.Generic;
using UnityEngine;

public class InteractionInput : MonoBehaviour
{
    // Lista que armazena os objetos interativos que estão dentro do alcance
    private List<IInteractable> objetosProximos = new List<IInteractable>();

    public void TentarInteragir()
    {
        if (objetosProximos.Count == 0) return;

        IInteractable objetoMaisProximo = null;
        float menorDistancia = Mathf.Infinity;
        Vector3 posicaoPlayer = transform.position;

        // Varre a lista para encontrar o objeto mais perto
        foreach (IInteractable interactable in objetosProximos)
        {
            float distancia = Vector3.Distance(posicaoPlayer, interactable.transform.position);

            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                objetoMaisProximo = interactable;
            }
        }

        // Executa a ação do objeto mais próximo
        if (objetoMaisProximo != null)
        {
            // 1. Pega a direção bruta do jogador para o objeto
            Vector3 direcaoBruta = (objetoMaisProximo.transform.position - transform.position).normalized;

            // 2. Alinha a direção para o eixo de 90º mais próximo (Forward, Back, Left, Right)
            Vector3 direcaoAlinhada = Vector3.zero;

            if (Mathf.Abs(direcaoBruta.x) > Mathf.Abs(direcaoBruta.z))
            {
                // Movimento no eixo X (Esquerda / Direita)
                direcaoAlinhada = direcaoBruta.x > 0 ? Vector3.right : Vector3.left;
            }
            else
            {
                // Movimento no eixo Z (Frente / Trás)
                direcaoAlinhada = direcaoBruta.z > 0 ? Vector3.forward : Vector3.back;
            }

            // 3. Envia a direção exata de 90º para o objeto
            objetoMaisProximo.Interagir(direcaoAlinhada);
        }
    }

    // Detecta quando um objeto interativo entra no alcance
    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && !objetosProximos.Contains(interactable))
        {
            objetosProximos.Add(interactable);
        }
    }

    // Detecta quando o objeto sai do alcance
    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && objetosProximos.Contains(interactable))
        {
            objetosProximos.Remove(interactable);
        }
    }
}
