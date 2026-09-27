using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject bossUIPanel;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private Image bossHealthFillImage; // Drag 'Boss_HealthFill' ke sini

    private float maxHealth;

    public void ActivateBossHealthBar(string bossName, float health)
    {
        maxHealth = health;
        if (bossUIPanel != null) bossUIPanel.SetActive(true);
        if (bossNameText != null) bossNameText.text = bossName;

        UpdateBossHealth(maxHealth);
    }

    public void UpdateBossHealth(float currentHealth)
    {
        if (bossHealthFillImage != null)
        {
            bossHealthFillImage.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        }
    }

    public void HideBossHealthBar()
    {
        if (bossUIPanel != null) bossUIPanel.SetActive(false);
    }
}