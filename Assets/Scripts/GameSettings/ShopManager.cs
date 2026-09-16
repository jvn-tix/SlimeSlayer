using UnityEngine;

public class ShopManager : MonoBehaviour, IInteractable
{
    [Header("UI Toko")]
    [SerializeField] private GameObject shopUI; // UI Toko

    public void Interact()
    {
        if (shopUI != null)
        {
            bool isActive = shopUI.activeSelf;
            shopUI.SetActive(!isActive); // Toggle UI Toko
            Time.timeScale = isActive ? 1f : 0f; // Pause game saat UI Toko aktif
        }
    }

    public bool CanInteract()
    {
        return true; // Selalu bisa berinteraksi dengan toko
    }
}
