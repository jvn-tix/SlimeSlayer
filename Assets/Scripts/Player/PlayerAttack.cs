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