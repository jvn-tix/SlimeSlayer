using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting; // Diperlukan untuk restart game

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
    [SerializeField] private TMP_Text coinText;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalCoinsText;

    [Header("Stage Progress")]
    public int currentStage = 1;
    public int maxStage = 3;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        UpdateCoinsUI();
    }

    // Fungsi menambah mata uang yang dipanggil saat pemain mengumpulkan koin
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        totalCoinsCollected += amount;
        UpdateCoinsUI();
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            UpdateCoinsUI();
            return true;
        }
        else
        {
            Debug.Log("Tidak cukup koin!");
            return false;
        }
    }

    void UpdateCoinsUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + currentCoins;
        }
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER! Total Koin Kamu: " + totalCoinsCollected);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalCoinsText != null)
        {
            finalCoinsText.text = "Final Coins: " + totalCoinsCollected;
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        currentCoins = 0;
        totalCoinsCollected = 0;
        currentStage = 1;
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
            // Tambahkan logika untuk memulai stage berikutnya
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
            Debug.Log("Attack upgraded to " + playerAttack);
            return true;
        }
        else
        {
            Debug.Log("Tidak cukup koin untuk upgrade attack!");
            return false;
        }
    }

    public bool upgradeMaxHealth(int cost, int amount)
    {
        if (SpendCoins(cost))
        {
            playerMaxHealth += amount;
            playerCurrentHealth += amount; // Juga menambah current health
            Debug.Log("Max Health upgraded to " + playerMaxHealth);
            return true;
        }
        else
        {
            Debug.Log("Tidak cukup koin untuk upgrade max health!");
            return false;
        }
    }

    public int GetAttackCost() { return attackUpgradeCost; }
    public int GetHealthCost() { return healthUpgradeCost; }
    public int GetCurrentCoins() { return currentCoins; }
}