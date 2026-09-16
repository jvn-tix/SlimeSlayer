using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk fungsi pindah scene

public class MapTransition : MonoBehaviour
{
    [Header("Pengaturan Pindah Scene")]
    [SerializeField] private string targetSceneName = "Lobby"; // Nama scene tujuan di Build Settings

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek apakah yang menginjak trigger adalah Player
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player masuk trigger, pindah ke: " + targetSceneName);

            // Pindah langsung ke scene tujuan
            SceneManager.LoadScene(targetSceneName);
        }
    }
}