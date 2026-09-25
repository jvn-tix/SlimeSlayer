using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Pengaturan Serangan")]
    public float attackRange = 1.5f;
    public float attackRate = 1.5f;
    public int damage = 1;

    private float nextAttackTime = 0f;
    private Transform player;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        FindPlayer();
    }

    void Update()
    {
        // Jika player belum ketemu (misal saat baru pindah scene), cari ulang
        if (player == null)
        {
            FindPlayer();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void AttackPlayer()
    {
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;

        if (anim != null)
        {
            anim.SetFloat("InputX", direction.x);
            anim.SetFloat("InputY", direction.y);
        }

        if (player.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("Slime berhasil menyerang Player!"); // Untuk tes di Console
        }
        else
        {
            Debug.LogWarning("Komponen PlayerHealth tidak ditemukan di GameObject Player!");
        }

        if (player.TryGetComponent(out Knockback knockback))
        {
            knockback.ApplyKnockback(transform);
        }
    }

    // Tampilkan jangkauan serangan di Scene View Unity agar mudah di-debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}