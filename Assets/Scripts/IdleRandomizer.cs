using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(WalkingInput))]
public class IdleRandomizer : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Animator animator;

    [Header("Tempo de Idle")]
    [SerializeField] private float delayParaIniciar = 3f;
    [SerializeField] private float intervaloEntreChances = 1.5f;
    [Range(0f, 1f)]
    [SerializeField] private float chanceDeAnimar = 0.5f;

    [Header("Triggers e Parâmetros do Animator")]
    [SerializeField] private string[] idleTriggers = { "DoBlink", "DoLookAround" };
    [SerializeField] private string walkingBool = "IsWalking";
    [SerializeField] private string groundedBool = "IsGrounded";

    private Rigidbody rb;
    private WalkingInput walkingInput;
    private Player player;
    private float tempoParado;
    private float proximaTentativa;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        walkingInput = GetComponent<WalkingInput>();
        player = GetComponent<Player>();

        if (animator == null)
        {
            animator = walkingInput.visual != null
                ? walkingInput.visual.GetComponentInChildren<Animator>()
                : GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        if (animator == null) return;

        bool noChao = player == null || player.IsGrounded;
        bool apertandoAndar = walkingInput.HasMoveInput;
        
        Vector3 velHorizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        bool rbMovendoHorizontal = velHorizontal.sqrMagnitude > 0.05f;

        bool estaAndando = noChao && (apertandoAndar || rbMovendoHorizontal);

        // Atualiza os parâmetros de movimento e chão no Animator
        animator.SetBool(groundedBool, noChao);
        animator.SetBool(walkingBool, estaAndando);

        if (!noChao || estaAndando)
        {
            ResetarIdle();
            return;
        }

        tempoParado += Time.deltaTime;

        if (tempoParado >= delayParaIniciar && Time.time >= proximaTentativa)
        {
            proximaTentativa = Time.time + intervaloEntreChances;

            if (Random.value <= chanceDeAnimar && idleTriggers.Length > 0)
            {
                string triggerSorteado = idleTriggers[Random.Range(0, idleTriggers.Length)];
                animator.SetTrigger(triggerSorteado);
                tempoParado = 0f;
            }
        }
    }

    private void ResetarIdle()
    {
        if (tempoParado > 0f || proximaTentativa > 0f)
        {
            tempoParado = 0f;
            proximaTentativa = 0f;

            foreach (string trigger in idleTriggers)
            {
                animator.ResetTrigger(trigger);
            }
        }
    }
}