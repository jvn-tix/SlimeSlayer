using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransition : MonoBehaviour
{
    [Header("Pengaturan Pindah Scene")]
    [SerializeField] private string targetSceneName = "Lobby";
    [SerializeField] private bool isReturnToLobby = false;

    // Cooldown agar tidak langsung ke-trigger saat spawn di dekat portal
    [SerializeField] private float transitionCooldown = 1.0f;
    private static float lastTransitionTime;
    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Pastikan yang masuk adalah Player
        if (collision.CompareTag("Player"))
        {
            // Cek apakah masih dalam masa cooldown dari transisi sebelumnya
            if (Time.time < lastTransitionTime + transitionCooldown)
            {
                return;
            }

            // Jika sedang proses transisi, cegah pemanggilan berulang
            if (isTransitioning) return;

            isTransitioning = true;
            lastTransitionTime = Time.time;

            Debug.Log("Player masuk portal, pindah ke: " + targetSceneName);

            if (isReturnToLobby && GameManager.instance != null)
            {
                GameManager.instance.CompleteCurrentStage();
            }

            // Panggil SceneController
            if (SceneController.Instance != null)
            {
                SceneController.Instance.LoadSceneByName(targetSceneName);
            }
            else
            {
                SceneManager.LoadScene(targetSceneName);
            }
        }
    }
}