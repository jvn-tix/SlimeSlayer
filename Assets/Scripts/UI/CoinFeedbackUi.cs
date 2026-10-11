using UnityEngine;
using TMPro;
using DG.Tweening; // Menggunakan namespace DOTween

public class CoinFeedbackUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Transform coinIcon;

    private int currentDisplayedCoin = 0;

    private void Start()
    {
        // Daftarkan UI ini ke GameManager otomatis saat scene dimuat
        if (GameManager.instance != null)
        {
            GameManager.instance.coinFeedbackUI = this; // Pastikan GameManager punya referensi variabel coinUI

            // Update tampilan awal koin saat scene mulai
            UpdateCoin(GameManager.instance.currentCoins);
        }
    }

    public void UpdateCoin(int newCoinAmount)
    {
        // 1. Icon Punch Scale
        if (coinIcon != null)
        {
            coinIcon.DOKill(true); // Hentikan animasi sebelumnya jika ada
            coinIcon.DOPunchScale(Vector3.one * 0.3f, 0.2f, vibrato: 1, elasticity: 0.5f);
        }

        // 2. Number Tick-Up
        if (coinText != null)
        {
            DOVirtual.Int(currentDisplayedCoin, newCoinAmount, 0.4f, (value) =>
            {
                currentDisplayedCoin = value;
                coinText.text = value.ToString();
            }).SetEase(Ease.OutQuad);
        }
    }
}