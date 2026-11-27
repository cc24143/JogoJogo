using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregarProximaFase : MonoBehaviour
{
    public void CarregarProximaFaseJogo(SceneAsset proxFase)
    {
        SceneManager.LoadScene(proxFase.name);
    }
}
