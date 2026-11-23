using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LensFlareComponentSRP))]
public class Portal : MonoBehaviour
{
    [Header("Proporções e Visual")]

    [Range(0f, 5f)] public float maxSizePortal = 5f;
    [Range(0f, 5f)] public float minSizePortal = 1f;

    [Header("Acesso Portal")]
    public int numAtivacoesMax = 1;
    public string proximoNivel;

    private int numAtivacoesAtual = 0;
    private bool isCallbackPortalAtivado = false;

    private float defaultStep = 0f;

    private LensFlareComponentSRP portalLensFlare;

    private bool piscandoDirecao = false;
    private bool colidindoPortal = false;

    void Start()
    {
        defaultStep = (minSizePortal + maxSizePortal) / numAtivacoesMax;

        portalLensFlare = GetComponent<LensFlareComponentSRP>();
        UpdateAparencia();
    }

    void OnTriggerEnter(Collider other) { colidindoPortal = true; }
    void OnTriggerExit(Collider other) { colidindoPortal = false; }

    void LateUpdate()
    {
        if (isCallbackPortalAtivado && colidindoPortal)
        {
            SceneManager.LoadScene(proximoNivel);
        }

        portalLensFlare.scale += (float) 0.0001 * (piscandoDirecao ? -1 : 1);

        if (portalLensFlare.scale >= 5.05 || portalLensFlare.scale <= 5)
        {
            piscandoDirecao = !piscandoDirecao;
            portalLensFlare.scale = (float) Math.Clamp(portalLensFlare.scale, 5, 5.05);
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
    }
}
