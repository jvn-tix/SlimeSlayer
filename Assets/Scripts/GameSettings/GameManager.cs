using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting; // Diperlukan untuk restart game

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Endless Score System")]
    private int currentScore = 0;
    [SerializeField] private TMP_Text scoreText;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if(gameOverPanel != null) gameOverPanel.SetActive(false);
        UpdateScoreUI();
    }

    // Fungsi menambah skor yang dipanggil saat musuh mati
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
    
    public void GameOver()
    {
        Debug.Log("GAME OVER! Skor Akhir Kamu: " + currentScore);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
            
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + currentScore;
        }
        
        Time.timeScale = 0f;
        //Contoh otomatis restart scene setelah player mati (opsional):
        //Invoke("RestartGame", 2f);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    // Fungsi pendukung untuk Toko Upgrade kemarin
    public int GetCurrentScore() { return currentScore; }
    public void ReduceScore(int amount) { currentScore -= amount; UpdateScoreUI(); }
}