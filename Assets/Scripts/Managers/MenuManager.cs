using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ComoJogar()
    {
        SceneManager.LoadScene("HowToPlayScreen");
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Saiu do jogo!");
    }
}