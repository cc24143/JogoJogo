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

    [Header("Triggers do Animator")]
    [SerializeField] private string[] idleTriggers = { "DoBlink", "DoLookAround" };

    private Rigidbody rb;
    private WalkingInput walkingInput;
    private float tempoParado;
    private float proximaTentativa;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        walkingInput = GetComponent<WalkingInput>();

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

        bool apertandoAndar = walkingInput.HasMoveInput;
        bool rbMovendo = rb.linearVelocity.sqrMagnitude > 0.05f;

        if (apertandoAndar || rbMovendo)
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