using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings (Default)")]
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;

    [Header("Events")]
    public UnityEvent<float> onHealthChanged;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 1. Sinkronkan nilai HP dari GameManager jika ada
        if (GameManager.instance != null)
        {
            maxHealth = GameManager.instance.playerMaxHealth;
            currentHealth = GameManager.instance.playerCurrentHealth;
        }
        else
        {
            currentHealth = maxHealth;
        }

        // Memberitahu UI nilai awal darah
        onHealthChanged.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // 2. Simpan sisa HP terbaru ke GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.playerCurrentHealth = currentHealth;
        }

        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine());

        // Panggil UI Health Bar/Heart
        onHealthChanged.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = Color.white;
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