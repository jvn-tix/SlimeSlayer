using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting; // Diperlukan untuk restart game

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    public int GetCurrentCoins() { return currentCoins; }
}