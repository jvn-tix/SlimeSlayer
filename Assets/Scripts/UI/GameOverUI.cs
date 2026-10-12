using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    private GameManager gameManager;
    public void RetryGame()
    {
        Time.timeScale = 1f;

        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadSceneByName("Lobby"); // Sesuaikan nama scene gameplay kamu
        }
        else
        {
            Debug.LogWarning("SceneController Instance NULL! Menggunakan SceneManager fallback.");
            SceneManager.LoadScene("Lobby");
        }

        gameManager.ResetFullGameProgress();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Tombol BackToMenu diklik!");

        if (SceneController.Instance != null)
        {
            Debug.Log("Memanggil SceneController.Instance.LoadSceneByName('Menu')");
            SceneController.Instance.LoadSceneByName("Menu"); // Sesuaikan nama scene menu kamu
        }
        else
        {
            Debug.LogWarning("SceneController Instance NULL! Menggunakan SceneManager fallback.");
            SceneManager.LoadScene("Menu"); // Fallback langsung jika SceneController hilang
        }
    }
}