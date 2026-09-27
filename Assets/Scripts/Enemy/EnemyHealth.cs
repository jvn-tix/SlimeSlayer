using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("UI Health Bar")]
    [SerializeField] private EnemyHealthBar enemyHealthBar; // Pasang jika ini Kroco
    private BossHealthBar bossHealthBar;                    // Diisi otomatis jika ini Boss

    [Header("Reward")]
    [SerializeField] private int coinReward = 1;

    [SerializeField] private float hitCameraShake = 0.6f;

    [SerializeField] private GameObject explodeVFX;

    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Update health bar kroco di awal
        if (enemyHealthBar != null)
        {
            enemyHealthBar.UpdateHealth(currentHealth, maxHealth);
        }
    }

    // Dipanggil oleh EnemySpawner saat Boss di-spawn
    public void SetBossHealthBar(BossHealthBar healthBar)
    {
        bossHealthBar = healthBar;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // --- UPDATE HEALTH BAR ---
        // 1. Jika ini Kroco
        if (enemyHealthBar != null)
        {
            enemyHealthBar.UpdateHealth(currentHealth, maxHealth);
        }

        // 2. Jika ini Boss
        if (bossHealthBar != null)
        {
            bossHealthBar.UpdateBossHealth(currentHealth);
        }

        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine());
        Debug.Log(gameObject.name + " kena hit! Sisa darah: " + currentHealth);

        if (CameraShake.instance != null)
        {
            CameraShake.instance.Shake(hitCameraShake);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    void Die()
    {
        Debug.Log("Musuh Mati!");

        if (explodeVFX != null)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.z = -1f;
            Instantiate(explodeVFX, spawnPosition, Quaternion.identity);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.AddCoins(coinReward);
        }

        Destroy(gameObject);
    }
}