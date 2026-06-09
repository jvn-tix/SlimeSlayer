using UnityEngine;
using UnityEngine.InputSystem; // WAJIB

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;
    public GameObject interactionIcon;

    // Tambahkan variabel ini untuk menangkap Project-Wide Actions Unity 6
    [SerializeField] private InputActionReference interactActionReference;

    void Start()
    {
        interactionIcon.SetActive(false);
    }

    // Fungsi ini otomatis berjalan kalau tombol E ditekan secara global
    private void OnEnable()
    {
        if (interactActionReference != null)
        {
            interactActionReference.action.started += OnInteractPressed;
        }
    }

    private void OnDisable()
    {
        if (interactActionReference != null)
        {
            interactActionReference.action.started -= OnInteractPressed;
        }
    }

    // Ini fungsi yang dipicu saat tombol ditekan
    private void OnInteractPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Sinyal tombol E berhasil masuk ke script!");

        if (interactableInRange != null)
        {
            interactableInRange.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;
            interactionIcon.SetActive(true);
            Debug.Log("NPC terdeteksi, siap berinteraksi.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
            interactionIcon.SetActive(false);
            Debug.Log("Meninggalkan NPC.");
        }
    }
}