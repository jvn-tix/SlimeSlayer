using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private Knockback knockback;
    [SerializeField] private Transform attackPoint;
    [Header("SFX")]
    [SerializeField] private AudioClip moveSFX;
    [SerializeField] [UnityEngine.Range(0f, 1f)] private float moveSFXVolume = 1f;
    private AudioSource audioSource;
    private float moveTimer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        HandleMoveSFX();
    }

    void FixedUpdate()
    {
        if (knockback != null && knockback.IsKnockedBack) return;

        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void HandleMoveSFX()
    {
        // Cek jika player sedang bergerak dan tidak terkena knockback
        bool isMoving = moveInput.sqrMagnitude > 0.01f && (knockback == null || !knockback.IsKnockedBack);

        if (isMoving)
        {
            // Jika suara belum jalan, putar audio dengan mode loop
            if (!audioSource.isPlaying && moveSFX != null)
            {
                audioSource.clip = moveSFX;
                audioSource.volume = moveSFXVolume;
                audioSource.loop = true; // Agar audio berulang otomatis
                audioSource.Play();
            }
        }
        else
        {
            // Jika player berhenti, langsung matikan suaranya
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
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