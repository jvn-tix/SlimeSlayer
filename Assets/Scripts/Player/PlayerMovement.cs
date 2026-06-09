using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    // Tambahkan referensi AttackPoint agar posisinya ikut berputar
    [SerializeField] private Transform attackPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput.sqrMagnitude > 0.01f) // Jika sedang menekan tombol arah
        {
            animator.SetBool("isWalking", true);

            // Update arah di Animator (InputX dan InputY tetap dipakai)
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);

            // Update posisi AttackPoint agar selalu di depan arah jalan
            if (attackPoint != null)
            {
                attackPoint.localPosition = moveInput.normalized * 0.5f;
            }
        }
        else if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            // Jangan update InputX/Y di sini supaya nilainya tetap di arah terakhir
        }
    }
}