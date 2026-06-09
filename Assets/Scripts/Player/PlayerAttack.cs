using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;

    [Header("Referensi")]
    public Transform attackPoint;
    public LayerMask enemyLayers;

    [Header("Pengaturan Serangan")]
    public float attackRange = 0.5f;
    public int attackDamage = 20;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        // Kita hanya ingin memicu serangan saat tombol pertama kali ditekan (Started)
        if (context.started)
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        anim.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out EnemyHealth health))
            {
                health.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}