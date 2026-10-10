using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthSlider;           // Slider utama
    [SerializeField] private Image delayedFillImage;      // Image lapis kedua di belakang (tarik gambar fill warna oranye/merah tua ke sini)

    [Header("Speed Settings")]
    [SerializeField] private float lerpSpeed = 5f;          // Kecepatan bar bayangan mengejar slider utama

    private float targetValue = 1f;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthSlider == null) return;

        // Set batas maksimal slider sesuai Max HP terbaru
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        // Hitung nilai normalisasi (0.0 sampai 1.0) untuk fillImage bayangan
        if (maxHealth > 0)
        {
            targetValue = currentHealth / maxHealth;
        }
    }

    void Update()
    {
        // Berjalan terus setiap frame untuk mengejar nilai target secara mulus (smooth lerp)
        if (delayedFillImage != null && healthSlider != null)
        {
            // Nilai fillAmount image bayangan disesuaikan dengan range slider (0 sampai 1)
            float currentDelayedFill = delayedFillImage.fillAmount;

            if (currentDelayedFill > targetValue)
            {
                // Menyusut perlahan saat player kena damage (catch-up effect)
                delayedFillImage.fillAmount = Mathf.Lerp(currentDelayedFill, targetValue, Time.deltaTime * lerpSpeed);
            }
            else
            {
                // Jika player nge-heal atau darah nambah, langsung samakan
                delayedFillImage.fillAmount = targetValue;
            }
        }
    }
}