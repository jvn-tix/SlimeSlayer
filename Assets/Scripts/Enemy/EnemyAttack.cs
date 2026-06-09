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
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void AttackPlayer()
    {
        // 1. Hitung arah ke Player agar Blend Tree tahu animasi mana yang diputar
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;

        anim.SetFloat("InputX", direction.x);
        anim.SetFloat("InputY", direction.y);

        // 2. Trigger Blend Tree Serangan
        //anim.SetTrigger("Attack");

        // 3. Beri Damage
        if (player.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }
    }
}