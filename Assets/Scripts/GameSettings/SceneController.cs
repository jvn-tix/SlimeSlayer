using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    [Header("UI Loading References")]
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private Slider progressBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. Paksakan waktu kembali normal
        Time.timeScale = 1f;

        // 2. Bersihkan fokus EventSystem agar tidak terkunci di UI Victory/GameOver lama
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        // 3. Garansi matikan Loading Panel dan LEPAS RAYCAST TARGET
        if (loadingScreenPanel != null)
        {
            loadingScreenPanel.SetActive(false);

            // Jika ada CanvasGroup, matikan blokir raycast-nya
            if (loadingScreenPanel.TryGetComponent(out CanvasGroup cg))
            {
                cg.blocksRaycasts = false;
                cg.interactable = false;
            }

            // Jika ada Image, matikan Raycast Target-nya saat loading selesai
            if (loadingScreenPanel.TryGetComponent(out Image img))
            {
                img.raycastTarget = false;
            }
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        Debug.Log("LoadSceneByName dipanggil: " + sceneName);
        Debug.Log("SceneController Instance: " + Instance);
   
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Nama scene belum diisi!");
            return;
        }

        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        Time.timeScale = 1f;

        if (loadingScreenPanel != null)
        {
            loadingScreenPanel.SetActive(true);

            // Aktifkan kembali Raycast saat loading berjalan
            if (loadingScreenPanel.TryGetComponent(out Image img))
            {
                img.raycastTarget = true;
            }
            if (loadingScreenPanel.TryGetComponent(out CanvasGroup cg))
            {
                cg.blocksRaycasts = true;
                cg.interactable = true;
            }
        }

        if (progressBar != null)
        {
            progressBar.value = 0f;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float currentProgress = 0f;
        bool isReadyToActivate = false;

        while (!isReadyToActivate)
        {
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Gunakan unscaledDeltaTime agar tidak macet jika waktu di-pause
            currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, Time.unscaledDeltaTime * 2f);

            if (progressBar != null)
            {
                progressBar.value = currentProgress;
            }

            if (operation.progress >= 0.9f && currentProgress >= 0.95f)
            {
                if (progressBar != null) progressBar.value = 1f;

                yield return new WaitForSecondsRealtime(0.2f);

                operation.allowSceneActivation = true;
                isReadyToActivate = true;
            }

            yield return null;
        }
    }

    public void ExitGame()
    {
        Debug.Log("Keluar dari game!");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}