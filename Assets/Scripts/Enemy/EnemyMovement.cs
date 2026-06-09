using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 3f;
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
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance > 0.1f)
            {
                // 1. Hitung arah gerak (Direction)
                Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;

                // 2. Gerakkan musuh
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

                // 3. Update Parameter Animator (InputX dan InputY)
                anim.SetFloat("InputX", direction.x);
                anim.SetFloat("InputY", direction.y);
                anim.SetBool("isMoving", true);
            }
            else
            {
                // Berhenti
                anim.SetBool("isMoving", false);
            }
        }
    }
}