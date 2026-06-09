using Unity.Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [Header("Referensi Kamera & Map")]
    [SerializeField] private CinemachineConfiner2D confiner;
    [SerializeField] private Collider2D mapBoundary;

    [Header("Teleportasi")]
    [SerializeField] private Transform spawnPoint;

    [Header("Enemy Spawner")]
    [SerializeField] private GameObject enemyPrefab; 
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private float spawnRate = 3f;

    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Pindahkan Kamera
            if (confiner != null && mapBoundary != null)
            {
                confiner.BoundingShape2D = mapBoundary;
                confiner.InvalidateBoundingShapeCache();
            }

            // 2. Teleport Player
            if (spawnPoint != null)
            {
                collision.transform.position = spawnPoint.position;
            }

            // 3. Spawn Musuh
            if (enemyPrefab != null && !hasSpawned)
            {
                InvokeRepeating("SpawnEnemies", 1f, spawnRate); // Mulai spawn setelah 1 detik, lalu ulang setiap spawnRate detik
                hasSpawned = true; // Biar nggak spawn terus-menerus kalau balik lagi
            }
        }
    }

    void SpawnEnemies()
    {
        // Munculkan musuh di setiap titik yang sudah kamu buat di Inspector
        foreach (Transform spot in enemySpawnPoints)
        {
            Instantiate(enemyPrefab, spot.position, Quaternion.identity);
        }
    }
}