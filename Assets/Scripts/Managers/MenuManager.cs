using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

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