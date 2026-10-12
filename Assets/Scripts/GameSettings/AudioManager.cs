using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource; // Audio Source khusus untuk SFX (buat komponen Audio Source kedua)

    [Header("Audio Clips - BGM")]
    [SerializeField] private AudioClip lobbyBGM;
    [SerializeField] private AudioClip gameplayBGM;

    [Header("Audio Clips - SFX")]
    [SerializeField] private AudioClip buttonClickSound; // Suara klik tombol
    [SerializeField] private AudioClip levelUpSound;     // Suara level up

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby")
        {
            PlayMusic(lobbyBGM);
        }
        else
        {
            PlayMusic(gameplayBGM);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // --- FUNGSI UNTUK MEMUTAR SFX ---
    public void PlayButtonClick()
    {
        if (sfxSource != null && buttonClickSound != null)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
    }

    public void PlayLevelUp()
    {
        if (sfxSource != null && levelUpSound != null)
        {
            sfxSource.PlayOneShot(levelUpSound);
        }
    }
}