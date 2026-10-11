using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("UI Health Bar")]
    [SerializeField] private EnemyHealthBar enemyHealthBar; // Pasang jika ini Kroco
    private BossHealthBar bossHealthBar;                    // Diisi otomatis jika ini Boss

    [Header("Coin Popup Settings")]
    [SerializeField] private GameObject coinIconPopupPrefab;
    [SerializeField] private int coinReward = 1;

    [SerializeField] private float hitCameraShake = 0.6f;

    [SerializeField] private GameObject explodeVFX;

    [SerializeField] private GameObject damagePopup;
    public static Transform canvasTransform;
    private bool isInvincible = false;
    private float invincibilityDuration = 0.2f;
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

        if(canvasTransform == null)
        {
            Canvas mainCanvas = FindFirstObjectByType<Canvas>();
            if (mainCanvas != null)
            {
                canvasTransform = mainCanvas.transform;
            }
            else
            {
                Debug.LogWarning("Canvas tidak ditemukan di Scene! Pastikan ada Canvas untuk UI.");
            }
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
        if (isInvincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        ShowDamagePopup(damage);

        StartCoroutine(InvincibilityRoutine());
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

    void ShowDamagePopup(int damage)
    {
        if (damagePopup != null)
        {
            // Spawn langsung di posisi musuh + sedikit ke atas (tanpa lewat Canvas utama)
            Vector3 spawnPos = transform.position + new Vector3(0, 1.2f, -0.1f);
            GameObject popupObj = Instantiate(damagePopup, spawnPos, Quaternion.identity);

            DamagePopup popup = popupObj.GetComponent<DamagePopup>();
            if (popup != null)
            {
                popup.Setup(damage);
            }
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

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        // Efek kedip visual (berkedip transparan atau merah)
        for (float i = 0; i < invincibilityDuration; i += 0.1f)
        {
            if (spriteRenderer != null)
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.4f); // Setengah transparan
            yield return new WaitForSeconds(0.1f);

            if (spriteRenderer != null)
                spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;

        isInvincible = false;
    }
    void Die()
    {
        Debug.Log("Musuh Mati!");

        // 1. Spawn VFX Ledakan (jika ada)
        if (explodeVFX != null)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.z = -1f;
            Instantiate(explodeVFX, spawnPosition, Quaternion.identity);
        }

        // 2. Spawn Coin Icon Popup di posisi musuh
        if (coinIconPopupPrefab != null)
        {
            Instantiate(coinIconPopupPrefab, transform.position, Quaternion.identity);
        }

        // 3. Tambahkan Koin ke GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.AddCoins(coinReward);
        }

        // 4. Hancurkan objek musuh
        Destroy(gameObject);
    }
}