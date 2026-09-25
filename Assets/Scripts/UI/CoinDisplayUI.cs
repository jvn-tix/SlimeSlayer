using UnityEngine;
using TMPro;

public class CoinDisplayUI : MonoBehaviour
{
    private TMP_Text coinText;

    void Awake()
    {
        coinText = GetComponent<TMP_Text>();
    }

    void Start()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (GameManager.instance != null && coinText != null)
        {
            coinText.text = "COINS : " + GameManager.instance.currentCoins;
        }
    }
}