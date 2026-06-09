using System.Collections;
using UnityEngine;
using UnityEngine.Events; // Wajib untuk UnityEvent
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 5;
    private float currentHealth;
    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;
    public UnityEvent<float> onHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        // Memberitahu UI nilai awal darah
        onHealthChanged.Invoke(currentHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRoutine());

        // Panggil semua UI yang terhubung
        onHealthChanged.Invoke(currentHealth);

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
        Debug.Log("Player Mati!");

        if (GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }
        Destroy(gameObject);
    }
}