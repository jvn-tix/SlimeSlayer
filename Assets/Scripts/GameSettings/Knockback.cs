using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isKnockedBack;

    [Header("Knockback Settings")]
    [SerializeField] private float defaultForce = 5f;
    [SerializeField] private float knockbackDuration = 0.15f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Panggil fungsi ini untuk memberikan gaya dorong ke karakter.
    /// attackerTransform = posisi penyerang (untuk menentukan arah dorongan)
    /// force = kekuatan dorongan opsional (jika -1, menggunakan defaultForce)
    /// </summary>
    public void ApplyKnockback(Transform attackerTransform, float force = -1f)
    {
        if (isKnockedBack || attackerTransform == null) return;

        float appliedForce = (force > 0) ? force : defaultForce;

        // Arah dorongan menjauh dari posisi penyerang
        Vector2 direction = (transform.position - attackerTransform.position).normalized;

        StartCoroutine(KnockbackRoutine(direction, appliedForce));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float force)
    {
        isKnockedBack = true;

        // Reset kecepatan agar dorongan terasa konsisten dan instan
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    public bool IsKnockedBack => isKnockedBack;
}