using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInput : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float cooldownInteracao = 0.8f;
    [SerializeField] private float tempoAlinhamento = 0.12f; // Deslize rápido antes de apertar
    [SerializeField] private float duracaoTotalAnimacao = 0.8f; // Tempo que o robô fica virado para o botão

    private List<IInteractable> objetosProximos = new List<IInteractable>();
    private float ultimaInteracao = -999f;
    private Transform playerRoot;

    private void Awake()
    {
        playerRoot = transform.root; // Pega o objeto pai "Robo"
        if (animator == null)
            animator = GetComponentInParent<Animator>();
    }

    public void TentarInteragir()
    {
        if (Time.time - ultimaInteracao < cooldownInteracao) return;
        if (objetosProximos.Count == 0) return;

        IInteractable objetoMaisProximo = null;
        float menorDistancia = Mathf.Infinity;
        Vector3 posicaoPlayer = playerRoot.position;

        foreach (IInteractable interactable in objetosProximos)
        {
            if (interactable is JumpOrb) continue;
            if (interactable is Button b && !b.PodeInteragir) continue; // Ignora botão já apertado

            float distancia = Vector3.Distance(posicaoPlayer, interactable.transform.position);
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                objetoMaisProximo = interactable;
            }
        }

        if (objetoMaisProximo != null)
        {
            ultimaInteracao = Time.time;

            // Se for um Button e tiver Ponto do Jogador configurado, alinha o robô
            if (objetoMaisProximo is Button botao && botao.pontoDoJogador != null)
                {
                    StopAllCoroutines();
                    StartCoroutine(AlinharRobo(botao.pontoDoJogador, botao.transform));
                }

            if (animator != null)
                animator.SetTrigger("DoClick");

            Vector3 direcaoBruta = (objetoMaisProximo.transform.position - playerRoot.position).normalized;
            Vector3 direcaoAlinhada = Mathf.Abs(direcaoBruta.x) > Mathf.Abs(direcaoBruta.z)
                ? (direcaoBruta.x > 0 ? Vector3.right : Vector3.left)
                : (direcaoBruta.z > 0 ? Vector3.forward : Vector3.back);

            objetoMaisProximo.Interagir(direcaoAlinhada);
        }
    }

    private IEnumerator AlinharRobo(Transform destino, Transform alvoBotao)
    {
        Vector3 posInicial = playerRoot.position;
        Vector3 posAlvo = new Vector3(destino.position.x, posInicial.y, destino.position.z);

        // Calcula o vetor horizontal apontando do ponto de encaixe direto para o botão,
        // respeitando o "up" atual do robô (funciona tanto na gravidade normal quanto na invertida!)
        Vector3 vetorParaBotao = alvoBotao.position - destino.position;
        Vector3 direcaoHorizontal = Vector3.ProjectOnPlane(vetorParaBotao, playerRoot.up).normalized;

        // Se o ponto estiver exatamente no centro do botão, usa a direção do InteractionPoint
        if (direcaoHorizontal.sqrMagnitude < 0.001f)
            direcaoHorizontal = Vector3.ProjectOnPlane(destino.forward, playerRoot.up).normalized;

        Quaternion rotInicial = playerRoot.rotation;
        Quaternion rotAlvo = Quaternion.LookRotation(-direcaoHorizontal, playerRoot.up);

        Rigidbody rb = playerRoot.GetComponent<Rigidbody>();

        float decorrido = 0f;
        while (decorrido < duracaoTotalAnimacao)
        {
            decorrido += Time.deltaTime;
            float t = Mathf.Clamp01(decorrido / tempoAlinhamento);

            if (rb != null)
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

            playerRoot.position = Vector3.Lerp(posInicial, posAlvo, t);
            // Segue impondo rotAlvo durante toda a animação para o WalkingInput não girar de volta
            playerRoot.rotation = Quaternion.Slerp(rotInicial, rotAlvo, t);

            yield return null;
        }
    }

    public bool TentarUsarOrb()
    {
        JumpOrb maisProximo = null;
        float menorDistancia = Mathf.Infinity;

        foreach (IInteractable interactable in objetosProximos)
        {
            if (interactable is not JumpOrb orb) continue;

            float distancia = Vector3.Distance(playerRoot.position, orb.transform.position);
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                maisProximo = orb;
            }
        }

        if (maisProximo == null) return false;

        maisProximo.Interagir(Vector3.zero);
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && !objetosProximos.Contains(interactable))
        {
            objetosProximos.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && objetosProximos.Contains(interactable))
        {
            objetosProximos.Remove(interactable);
        }
    }
}