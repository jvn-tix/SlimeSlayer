using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [HideInInspector] public int maxHealth = 10;
    [HideInInspector] public int currentHealth = 10;

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
        HealthBarUI healthBar = FindObjectOfType<HealthBarUI>();
        if (healthBar != null)
        {
            onHealthChanged.RemoveAllListeners();
            onHealthChanged.AddListener(healthBar.UpdateHealthBar);
            onHealthChanged.Invoke(currentHealth, maxHealth); // Refresh visual slider
        }
    }

    public void TakeDamage(int damage)
    {
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
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = Color.white;
        }
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