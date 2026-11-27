using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayManager : MonoBehaviour
{
    public void Voltar()
    {
        SceneManager.LoadScene("Menu");
    }
}