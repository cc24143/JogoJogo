using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour, IInteractable
{
    [Header("Evento disparado ao apertar o botão")]
    public UnityEvent aoApertar;

    [Header("Alinhamento do Jogador")]
    [Tooltip("Arraste o objeto vazio PontoInteracao posicionado na frente do botão")]
    public Transform pontoDoJogador;

    [Header("Opções")]
    [SerializeField] private bool apenasUmaVez = false;
    [SerializeField] private float cooldown = 0.8f;
    [SerializeField] private float atrasoParaAtivar = 0.47f;

    [Header("Movimento e Visual do Botão")]
    [SerializeField] private Transform parteMovel;
    [SerializeField] private Vector3 deslocamentoLocal = new Vector3(0, 0, -0.1f);
    [SerializeField] private float velocidade = 10f;
    [SerializeField] private Material materialPressionado; // Arraste o ButtonPressedMaterial aqui

    private bool jaUsado;
    private float ultimoUso = -999f;
    private Vector3 posicaoInicial;
    private Coroutine animacaoAtual;

    private Renderer rendererBotao;
    private Material materialOriginal;

    private void Start()
    {
        if (parteMovel == null && transform.childCount > 0)
            parteMovel = transform.GetChild(0);

        if (parteMovel != null)
        {
            posicaoInicial = parteMovel.localPosition;
            rendererBotao = parteMovel.GetComponentInChildren<Renderer>();

            if (rendererBotao != null)
                materialOriginal = rendererBotao.material;
        }
    }

    public bool PodeInteragir
    {
        get
        {
            if (apenasUmaVez && jaUsado) return false;
            if (Time.time - ultimoUso < cooldown) return false;
            return true;
        }
    }

    public void Interagir(Vector3 direction)
    {
        if (!PodeInteragir) return;

        ultimoUso = Time.time;
        jaUsado = true;

        if (animacaoAtual != null) StopCoroutine(animacaoAtual);
        animacaoAtual = StartCoroutine(SequenciaApertar());
    }

    private IEnumerator SequenciaApertar()
    {
        if (atrasoParaAtivar > 0f)
            yield return new WaitForSeconds(atrasoParaAtivar);

        // Troca para o material pressionado assim que a mão encosta
        if (rendererBotao != null && materialPressionado != null)
            rendererBotao.material = materialPressionado;

        aoApertar?.Invoke();

        if (parteMovel != null)
        {
            Vector3 posicaoPressionada = posicaoInicial + deslocamentoLocal;

            while (Vector3.Distance(parteMovel.localPosition, posicaoPressionada) > 0.001f)
            {
                parteMovel.localPosition = Vector3.MoveTowards(parteMovel.localPosition, posicaoPressionada, velocidade * Time.deltaTime);
                yield return null;
            }
            parteMovel.localPosition = posicaoPressionada;

            // Se não for "Apenas Uma Vez", volta posição e material originais
            if (!apenasUmaVez)
            {
                yield return new WaitForSeconds(0.15f);

                if (rendererBotao != null && materialOriginal != null)
                    rendererBotao.material = materialOriginal;

                while (Vector3.Distance(parteMovel.localPosition, posicaoInicial) > 0.001f)
                {
                    parteMovel.localPosition = Vector3.MoveTowards(parteMovel.localPosition, posicaoInicial, velocidade * Time.deltaTime);
                    yield return null;
                }
                parteMovel.localPosition = posicaoInicial;
            }
        }
    }
}