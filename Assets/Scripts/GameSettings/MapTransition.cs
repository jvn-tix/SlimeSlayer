using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk fungsi pindah scene

public class MapTransition : MonoBehaviour
{
    [Header("Pengaturan Pindah Scene")]
    [SerializeField] private string targetSceneName = "Lobby";
    [SerializeField] private bool isReturnToLobby = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek apakah yang menginjak trigger adalah Player
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player masuk trigger, pindah ke: " + targetSceneName);

            if(isReturnToLobby && GameManager.instance != null)
            {
                // Jika ingin kembali ke Lobby, set currentStage ke 1
                GameManager.instance.CompleteCurrentStage();
            }
            
            SceneManager.LoadScene(targetSceneName);
        }
    }
}