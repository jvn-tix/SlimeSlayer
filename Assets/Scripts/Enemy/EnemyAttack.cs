using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Pengaturan Serangan")]
    public float attackRange = 0.8f;
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
        // 1. Hitung arah ke Player untuk Blend Tree
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;

        if (anim != null)
        {
            anim.SetFloat("InputX", direction.x);
            anim.SetFloat("InputY", direction.y);
        }

        // 2. Beri Damage (Pencarian aman menggunakan TryGetComponent)
        if (player.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }
    }

    // Tampilkan jangkauan serangan di Scene View Unity agar mudah di-debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}