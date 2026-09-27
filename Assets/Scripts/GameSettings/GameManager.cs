using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Stats")]
    public int playerAttack = 3;
    public int playerMaxHealth = 10;
    public int playerCurrentHealth = 10;

    [Header("Shop Settings")]
    [SerializeField] private int attackUpgradeCost = 5;
    [SerializeField] private int healthUpgradeCost = 5;

    [Header("Currency")]
    public int currentCoins = 0;
    public int totalCoinsCollected = 0;

    [Header("Stage Progress")]
    public int currentStage = 1;
    public int maxStage = 3;

    private GameObject currentGameOverPanel;
    private GameObject currentVictoryPanel;

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

    // Dipanggil otomatis setiap kali scene baru selesai dimuat
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;

        // Reset referensi panel UI agar tidak menyimpan objek lama yang sudah hancur
        currentGameOverPanel = null;
        currentVictoryPanel = null;

        // Jika kembali ke scene Menu Utama, otomatis bersihkan progres game
        if (scene.name == "Menu")
        {
            ResetFullGameProgress();
        }
    }

    // -------------------------------------------------------------
    // LOGIKA HEALTH & PROGRES
    // -------------------------------------------------------------

    // Memulihkan nyawa player kembali penuh
    public void ResetPlayerHealth()
    {
        playerCurrentHealth = playerMaxHealth;
        Debug.Log("Nyawa player berhasil di-reset penuh: " + playerCurrentHealth);
    }

    // Reset seluruh progres jika ulang dari awal
    public void ResetFullGameProgress()
    {
        currentCoins = 0;
        totalCoinsCollected = 0;
        currentStage = 1;
        ResetPlayerHealth();
    }

    // -------------------------------------------------------------
    // CURRENCY & UI
    // -------------------------------------------------------------

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        totalCoinsCollected += amount;
        NotifyCoinUI();
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            NotifyCoinUI();
            return true;
        }
        else
        {
            Debug.Log("Tidak cukup koin!");
            return false;
        }
    }

    public void NotifyCoinUI()
    {
        CoinDisplayUI coinUI = FindFirstObjectByType<CoinDisplayUI>();
        if (coinUI != null)
        {
            coinUI.UpdateDisplay();
        }
    }

    // -------------------------------------------------------------
    // REGISTRASI PANEL UI
    // -------------------------------------------------------------

    public void RegisterGameOverPanel(GameObject panel)
    {
        currentGameOverPanel = panel;
        if (currentGameOverPanel != null)
        {
            currentGameOverPanel.SetActive(false);
        }
    }

    public void RegisterVictoryPanel(GameObject panel)
    {
        currentVictoryPanel = panel;
        if (currentVictoryPanel != null)
        {
            currentVictoryPanel.SetActive(false);
        }
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER DIPANGGIL!");

        if (currentGameOverPanel != null)
        {
            Debug.Log("GameOverPanel ditemukan!");
            currentGameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("GameOverPanel belum terdaftar!");
        }

        Time.timeScale = 0f;
    }

    public void Victory()
    {
        Debug.Log("VICTORY DIPANGGIL!");

        if (currentVictoryPanel != null)
        {
            currentVictoryPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("VictoryPanel belum terdaftar!");
        }

        Time.timeScale = 0f;
    }

    // -------------------------------------------------------------
    // NAVIGASI SCENE
    // -------------------------------------------------------------

    public void RestartGame()
    {
        Time.timeScale = 1f;
        ResetFullGameProgress(); // Reset nyawa & koin saat retry

        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadSceneByName("Lobby"); // Menggunakan SceneController async jika ada
        }
        else
        {
            SceneManager.LoadScene("Lobby");
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        ResetFullGameProgress(); // Reset progres saat ke menu utama

        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadSceneByName("Menu");
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void CompleteCurrentStage()
    {
        if (currentStage < maxStage)
        {
            currentStage++;
            Debug.Log("Stage " + currentStage + " dimulai!");
            // playerCurrentHealth sengaja tidak di-reset agar sisa nyawa dibawa ke stage berikutnya
        }
        else
        {
            Debug.Log("Selamat! Kamu telah menyelesaikan semua stage!");
        }
    }

    // -------------------------------------------------------------
    // UPGRADES
    // -------------------------------------------------------------

    public bool upgradeAttack(int cost, int amount)
    {
        if (SpendCoins(cost))
        {
            playerAttack += amount;
            return true;
        }
        return false;
    }

    public bool upgradeMaxHealth(int cost, int amount)
    {
        if (SpendCoins(cost))
        {
            playerMaxHealth += amount;
            playerCurrentHealth = playerMaxHealth; // Isi darah penuh saat memperbesar kapasitas darah
            return true;
        }
        return false;
    }

    public int GetAttackCost() { return attackUpgradeCost; }
    public int GetHealthCost() { return healthUpgradeCost; }
    public int GetCurrentCoins() { return currentCoins; }
}