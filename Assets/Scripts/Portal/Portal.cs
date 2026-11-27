using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(LensFlareComponentSRP))]
public class Portal : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds5 = new(5);
    
    [Header("Proporções e Visual")]
    [Range(0f, 5f)] public float maxSizePortal = 5f;
    [Range(0f, 5f)] public float minSizePortal = 1f;
    [Range(0f, 1f)] public float taxaBreathePortal = 0.2f;
    public Color maxColorPortal = Color.white;
    public Color minColorPortal = Color.black;

    [Header("Acesso Portal")]
    [Min(0)] public int numAtivacoesMax = 1;
    [Min(0)] public float velocidadeCrescimento = 1;

    [Header("Ação Colisão/Ativação")]
    public UnityEvent acaoColisao = null;

    private int numAtivacoesAtual = -1;
    private bool callbackExecutado = false;

    private float targetIntensity;
    private float targetSize;
    private Color targetColor;

    private Canvas canvas;
    private LensFlareComponentSRP lensFlare;
    private new Renderer renderer;

    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        lensFlare = GetComponent<LensFlareComponentSRP>();
        renderer = GetComponent<Renderer>();

        AtivarSource();

        canvas.enabled = false;

        transform.localScale = Vector3.one * targetSize;
        renderer.material.color = targetColor;
    }

    void Start()
    {
        StartCoroutine(AnimacaoPortal());
    }

    void OnTriggerEnter(Collider other)
    {
        TentarCallback();
    }

    public void AtivarSource()
    {
        if (numAtivacoesMax <= 0)
        {
            targetIntensity = 0.5f;
            targetSize = maxSizePortal;
            targetColor = maxColorPortal;
        }
        else
        {
            numAtivacoesAtual = Math.Min(numAtivacoesAtual + 1, numAtivacoesMax);

            var prog = numAtivacoesAtual / (float) numAtivacoesMax;

            targetIntensity = 0.5f * prog;
            targetSize = Mathf.Lerp(minSizePortal, maxSizePortal, prog);
            targetColor = Color.Lerp(minColorPortal, maxColorPortal, prog);
        }

        if (numAtivacoesAtual == numAtivacoesMax || numAtivacoesMax <= 0)
        {
            canvas.enabled = true;

            StartCoroutine(EscondeCanvas());
        }
    }

    private IEnumerator EscondeCanvas()
    {
        yield return _waitForSeconds5;

        canvas.enabled = false;
    }

    private void TentarCallback()
    {
        if (!callbackExecutado && numAtivacoesAtual >= numAtivacoesMax)
        {
            callbackExecutado = true;
            acaoColisao?.Invoke();
        }
    }

    private IEnumerator AnimacaoPortal()
    {
        float currentCycleBreathe;

        while (true)
        {
            var currentSize = transform.localScale.x;
            var currentColor = renderer.material.color;

            if (Mathf.Abs(currentSize - targetSize) > taxaBreathePortal)
            {
                var proxSize = Mathf.MoveTowards(currentSize, targetSize, Time.deltaTime * velocidadeCrescimento);

                transform.localScale = proxSize * Vector3.one;
                renderer.material.color = Color.Lerp(currentColor, targetColor, Mathf.Clamp01(Time.deltaTime * velocidadeCrescimento));
            }
            else
            {
                renderer.material.color = targetColor;
                lensFlare.intensity = targetIntensity;

                currentCycleBreathe = Time.time * velocidadeCrescimento;
                transform.localScale = (targetSize + (float) Math.Sin(currentCycleBreathe) * taxaBreathePortal) * Vector3.one;
            }

            yield return null;
        }
    }
}