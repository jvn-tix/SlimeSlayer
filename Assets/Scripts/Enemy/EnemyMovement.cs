using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Pengaturan Pergerakan")]
    public float speed = 2.5f;
    public float stopDistance = 0.6f; // Musuh berhenti mengejar jika sudah dalam jangkauan serang

    private Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    private Knockback knockback;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();

        FindPlayer();
    }

    void Update()
    {
        if(knockback != null && knockback.IsKnockedBack) return;

        if (player == null)
        {
            FindPlayer();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // Hanya bergerak jika posisi player di luar jarak stopDistance
        if (distance > stopDistance)
        {
            Vector2 direction = ((Vector2)player.position - rb.position).normalized;

            if (anim != null)
            {
                anim.SetFloat("InputX", direction.x);
                anim.SetFloat("InputY", direction.y);
                anim.SetBool("isMoving", true);
            }
        }
        else
        {
            if (anim != null)
            {
                anim.SetBool("isMoving", false);
            }
        }
    }

    void FixedUpdate()
    {
        if(knockback != null && knockback.IsKnockedBack) return;

        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance > stopDistance)
            {
                Vector2 targetPosition = Vector2.MoveTowards(rb.position, player.position, speed * Time.fixedDeltaTime);
                rb.MovePosition(targetPosition);
            }
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
}