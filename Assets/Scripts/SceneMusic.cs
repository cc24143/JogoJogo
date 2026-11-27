using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusic : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public AudioClip endingMusic;

    private AudioSource audioSource;
    private static SceneMusic instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    void PlayMusicForScene(string scene)
    {
        AudioClip newClip = null;

        if (scene == "Menu" || scene == "HowToPlayScreen")
            newClip = menuMusic;
        else if (scene == "Level1" || scene == "Level2" || scene == "Level3")
            newClip = gameplayMusic;
        else if (scene == "EndScreen")
            newClip = endingMusic;

        if (newClip == null) return;

        if (audioSource.clip == newClip && audioSource.isPlaying) return;

        audioSource.clip = newClip;
        audioSource.Play();
    }
}