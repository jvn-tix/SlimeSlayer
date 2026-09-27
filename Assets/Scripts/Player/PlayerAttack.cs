using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;

    [Header("Referensi")]
    public Transform attackPoint;
    public LayerMask enemyLayers;

    [Header("Pengaturan Serangan Default")]
    public float attackRange = 0.5f;
    public int defaultAttackDamage = 3;
    [Header("SFX")]
    [SerializeField]private AudioClip attackSFX;
    [SerializeField] [UnityEngine.Range(0f, 1f)] private float attackSFXVolume = 1f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }

        if(attackSFX != null)
        {
            AudioSource.PlayClipAtPoint(attackSFX, transform.position, attackSFXVolume);
        }

        // Ambil stat attack dari GameManager, kalau null pakai nilai default
        int currentDamage = (GameManager.instance != null) ? GameManager.instance.playerAttack : defaultAttackDamage;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out EnemyHealth health))
            {
                health.TakeDamage(currentDamage);
            }

            if(enemy.TryGetComponent(out Knockback knockback))
            {
                knockback.ApplyKnockback(transform);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}