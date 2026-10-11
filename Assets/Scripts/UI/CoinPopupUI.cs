using UnityEngine;
using DG.Tweening; // Menggunakan DOTween

public class CoinPopup : MonoBehaviour
{
    [Header("Popup Settings")]
    [SerializeField] private float floatDistance = 1.2f;
    [SerializeField] private float jumpPower = 0.8f;
    [SerializeField] private float duration = 0.8f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("CoinPopup script membutuhkan SpriteRenderer!");
            Destroy(gameObject);
            return;
        }

        // Pentalan acak ke sumbu X
        float randomX = Random.Range(-0.8f, 0.8f);
        Vector3 endPosition = transform.position + new Vector3(randomX, floatDistance, 0f);

        // Animasi Loncatan (Tanpa mengubah scale awal di skrip, ikuti ukuran asli prefab)
        transform.DOJump(endPosition, jumpPower, 1, duration)
            .SetEase(Ease.OutQuad);

        // Animasi Fade Out
        spriteRenderer.DOFade(0f, duration * 0.9f)
            .SetEase(Ease.InQuad)
            .SetDelay(duration * 0.1f)
            .OnComplete(() => Destroy(gameObject));
    }
}