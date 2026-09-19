using UnityEngine;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [Header("Text Info Stats")]
    [SerializeField] private TMP_Text attackStatText;  // "ATK: 10"
    [SerializeField] private TMP_Text healthStatText;  // "Max HP: 100"

    [Header("Text Costs")]
    [SerializeField] private TMP_Text attackCostText;  // "10 Coins"
    [SerializeField] private TMP_Text healthCostText;  // "15 Coins"

    void OnEnable()
    {
        // Otomatis update angka begitu panel toko terbuka
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (GameManager.instance == null) return;

        if (attackStatText != null)
            attackStatText.text = "ATK: " + GameManager.instance.playerAttack;

        if (healthStatText != null)
            healthStatText.text = "HP: " + GameManager.instance.playerMaxHealth;

        if (attackCostText != null)
            attackCostText.text = GameManager.instance.GetAttackCost().ToString();
        if (healthCostText != null)
            healthCostText.text = GameManager.instance.GetHealthCost().ToString();
    }

    // Dipanggil oleh Button OnClick Upgrade ATK
    public void BuyAttack()
    {
        if (GameManager.instance != null)
        {
            int cost = GameManager.instance.GetAttackCost();
            int amount = 1;
            GameManager.instance.upgradeAttack(cost, amount);
            UpdateDisplay();
        }
    }

    // Dipanggil oleh Button OnClick Upgrade HP
    public void BuyHealth()
    {
        if (GameManager.instance != null)
        {
            int cost = GameManager.instance.GetHealthCost();
            int amount = 2;
            GameManager.instance.upgradeMaxHealth(cost, amount);
            UpdateDisplay();
        }
    }
}