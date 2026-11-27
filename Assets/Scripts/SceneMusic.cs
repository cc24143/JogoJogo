
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusic : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public AudioClip endingMusic;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayMusic();
    }

    void PlayMusic()
    {
        string scene = SceneManager.GetActiveScene().name;

        if (scene == "Menu" || scene == "HowToPlayScreen")
            audioSource.clip = menuMusic;

        else if (scene == "Level1" ||
                 scene == "Level2" ||
                 scene == "Level3")
            audioSource.clip = gameplayMusic;

        else if (scene == "EndScreen")
            audioSource.clip = endingMusic;

        audioSource.loop = true;
        audioSource.Play();
    }
}
