using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    // Fungsi ini yang akan dipanggil oleh event di PlayerHealth.cs
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthSlider == null) return;

        // Set batas maksimal slider sesuai Max HP terbaru (misal: 10, 15, 20)
        healthSlider.maxValue = maxHealth;

        // Set isi slider sesuai HP pemain saat ini
        healthSlider.value = currentHealth;
    }
}