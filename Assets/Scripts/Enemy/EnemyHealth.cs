using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private Animator anim;

    [Header("Reward")]
    [SerializeField] private int coinReward = 1;

    [SerializeField] private float hitCameraShake = 0.6f;

    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(flashCoroutine != null) StopCoroutine(flashCoroutine);
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

        if (GameManager.instance != null) { 
            GameManager.instance.AddCoins(coinReward); 
        }


        Destroy(gameObject);
    }
}