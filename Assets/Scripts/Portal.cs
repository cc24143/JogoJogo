using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(LensFlareComponentSRP))]
public class Portal : MonoBehaviour
{
    [Header("Proporções e Visual")]

    [Range(0f, 1f)] public float maxSizePortal = 1f;
    [Range(0f, 1f)] public float minSizePortal = 0.2f;
    public Color initialColorPortal = Color.black;
    public Color finalColorPortal = Color.white;

    [Header("Acesso Portal")]
    public int numAtivacoesMax = 1;
    public UnityEvent callbackPortalAtivado;

    private int numAtivacoesAtual = 0;
    private bool isCallbackPortalAtivado = false;

    private float defaultStep = 0f;

    private Light portalLight;
    private LensFlareComponentSRP portalLensFlare;

    private bool piscandoDirecao = false;

    void Start()
    {
        defaultStep = (minSizePortal + maxSizePortal) / numAtivacoesMax;

        portalLight = GetComponent<Light>();
        portalLensFlare = GetComponent<LensFlareComponentSRP>();

        UpdateAparencia();
    }

    void LateUpdate()
    {
        if (isCallbackPortalAtivado)
        {
            callbackPortalAtivado?.Invoke();
            callbackPortalAtivado = null;
        }

        portalLensFlare.scale += (float) 0.0001 * (piscandoDirecao ? -1 : 1);

        if (portalLensFlare.scale >= 1 || portalLensFlare.scale <= 0.95)
        {
            piscandoDirecao = !piscandoDirecao;
            portalLensFlare.scale = (float) Math.Clamp(portalLensFlare.scale, 0.95, 1);
        }  
    }

    public void AtivarSource()
    {
        if (++numAtivacoesAtual >= numAtivacoesMax)
        {
            isCallbackPortalAtivado = true;
        }

        UpdateAparencia();
    }

    private void UpdateAparencia()
    {
        float currentAtiv = minSizePortal + Math.Min(numAtivacoesAtual, numAtivacoesMax) * defaultStep;
        transform.localScale = new Vector3(currentAtiv, currentAtiv, currentAtiv);

        Color currentCol = Color.Lerp(initialColorPortal, finalColorPortal, Math.Min(numAtivacoesAtual, numAtivacoesMax) / numAtivacoesMax);
        portalLight.color = currentCol;
    }
}
