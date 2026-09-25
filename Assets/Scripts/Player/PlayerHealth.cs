using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [HideInInspector] public int maxHealth = 10;
    [HideInInspector] public int currentHealth = 10;

    [Header("I-Frames Settings")]
    [SerializeField] private float iFrameDuration = 0.8f;
    [SerializeField] private int numberOfFlashes = 5;
    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;

    [Header("Events")]
    public UnityEvent<float, float> onHealthChanged;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 1. Ambil data HP dari GameManager
        if (GameManager.instance != null)
        {
            maxHealth = GameManager.instance.playerMaxHealth;
            currentHealth = GameManager.instance.playerCurrentHealth;
        }

        // 2. Hubungkan secara otomatis ke HealthBarUI di Canvas scene aktif
        AutoConnectHealthBarUI();
    }

    public void AutoConnectHealthBarUI()
    {
        HealthBarUI healthBar = FindFirstObjectByType<HealthBarUI>();
        if (healthBar != null)
        {
            onHealthChanged.RemoveAllListeners();
            onHealthChanged.AddListener(healthBar.UpdateHealthBar);
            onHealthChanged.Invoke(currentHealth, maxHealth); // Refresh visual slider
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // Simpan sisa HP ke GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.playerCurrentHealth = currentHealth;
        }

        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine());

        onHealthChanged.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        if(CameraShake.instance != null)
        {
            CameraShake.instance.Shake(1f);
        }
    }

    public void RefreshHealthFromGameManager()
    {
        if (GameManager.instance != null)
        {
            maxHealth = GameManager.instance.playerMaxHealth;
            currentHealth = GameManager.instance.playerCurrentHealth;
            onHealthChanged.Invoke(currentHealth, maxHealth);
        }
    }

    private IEnumerator FlashRoutine()
    {
        isInvincible = true;

        if (spriteRenderer != null)
        {
            float flashInterval = iFrameDuration / (numberOfFlashes * 2);
            for (int i = 0; i < numberOfFlashes; i++)
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(flashInterval);
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(flashInterval);
            }
        }
        else
        {
            yield return new WaitForSeconds(iFrameDuration);
        }
        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("Player Mati!");

        if (GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }

        Destroy(gameObject);
    }
}