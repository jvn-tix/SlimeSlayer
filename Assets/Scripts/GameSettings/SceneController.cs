using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk mengatur perpindahan scene

public class SceneController : MonoBehaviour
{
    // Fungsi untuk masuk ke dalam game
    public void PlayGame()
    {
        Debug.Log("Masuk ke dalam game...");


        SceneManager.LoadScene("Gameplay");
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