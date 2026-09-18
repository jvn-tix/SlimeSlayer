using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    [SerializeField] private GameObject normalEnemy;
    [SerializeField] private GameObject bossEnemy;


    [Header("Enemy Settings")]
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int baseEnemiesCount = 5;

    [Header("Portal Settings")]
    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private Transform portalSpawnPoint;

    private int activeEnemies = 0;
    private bool portalSpawned = false;
    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        int stage = 1;
        int maxStage = 3;

        if (GameManager.instance != null)
        {
            stage = GameManager.instance.currentStage;
            maxStage = GameManager.instance.maxStage;
        }

        if (stage == maxStage)
        {
            SpawnBoss();
        }
        else
        {
            SpawnNormals(stage);
        }
    }

    void SpawnNormals(int stage)
    {
        int totalEnemies = stage * baseEnemiesCount;
        activeEnemies = totalEnemies;

        for (int i = 0; i < totalEnemies; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject newEnemy = Instantiate(normalEnemy, spawnPoint.position, Quaternion.identity);

            EnemyTracker tracker = newEnemy.AddComponent<EnemyTracker>();
            tracker.spawner = this;
        }
    }

    void SpawnBoss()
    {
        activeEnemies = 1;

        Vector3 spawnPosition = (bossSpawnPoint != null) ? bossSpawnPoint.position : spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        GameObject newBoss = Instantiate(bossEnemy, spawnPosition, Quaternion.identity);

        EnemyTracker tracker = newBoss.AddComponent<EnemyTracker>();
        tracker.spawner = this;
    }

    public void onEnemyDefeated()
    {
        activeEnemies--;
        if (activeEnemies <= 0 && !portalSpawned)
        {
            SpawnPortal();
        }
    }

    public void SpawnPortal()
    {
        portalSpawned = true;

        if(portalPrefab != null)
        {
            Vector3 spawnPos = (portalSpawnPoint != null) ? portalSpawnPoint.position : Vector3.zero;
            Instantiate(portalPrefab, spawnPos, Quaternion.identity);
        }
    }
}

public class EnemyTracker : MonoBehaviour
{
    [HideInInspector] public EnemySpawner spawner;
    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.onEnemyDefeated();
        }
    }
}
