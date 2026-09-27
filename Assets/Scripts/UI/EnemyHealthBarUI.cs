using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthFillImage; // Drag GameObject 'HealthBar_Fill' ke sini

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthFillImage != null)    
        {
            // Menghitung rasio darah 0.0 sampai 1.0
            healthFillImage.fillAmount = currentHealth / maxHealth;
        }
    }
}