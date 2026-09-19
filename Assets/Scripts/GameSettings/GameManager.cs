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

    // Memberitahu script CoinDisplayUI di scene aktif untuk update teks
    public void NotifyCoinUI()
    {
        CoinDisplayUI coinUI = FindFirstObjectByType<CoinDisplayUI>();
        if (coinUI != null)
        {
            coinUI.UpdateDisplay();
        }
    }

    public void RegisterGameOverPanel(GameObject panel)
    {
        currentGameOverPanel = panel;
        currentGameOverPanel.SetActive(false);
    }

    public void RegisterVictoryPanel(GameObject panel)
    {
        currentVictoryPanel = panel;
        currentVictoryPanel.SetActive(false);
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

    public void RestartGame()
    {
        Time.timeScale = 1f;
        currentCoins = 0;
        totalCoinsCollected = 0;
        currentStage = 1;

        playerCurrentHealth = playerMaxHealth;
        SceneManager.LoadScene("Lobby");
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void CompleteCurrentStage()
    {
        if (currentStage < maxStage)
        {
            currentStage++;
            Debug.Log("Stage " + currentStage + " dimulai!");
        }
        else
        {
            Debug.Log("Selamat! Kamu telah menyelesaikan semua stage!");
        }
    }

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
            playerCurrentHealth = playerMaxHealth;
            return true;
        }
        return false;
    }

    public int GetAttackCost() { return attackUpgradeCost; }
    public int GetHealthCost() { return healthUpgradeCost; }
    public int GetCurrentCoins() { return currentCoins; }
}