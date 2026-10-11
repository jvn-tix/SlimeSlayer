using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    private float disappearTimer = 0.8f;

    // Simpan warna atas dan bawah secara terpisah untuk efek gradient & fade
    private Color topColor;
    private Color bottomColor;

    public void Setup(int damageAmount)
    {
        if (textMesh == null) textMesh = GetComponentInChildren<TextMeshProUGUI>();

        textMesh.text = damageAmount.ToString();
        textMesh.fontSize = 36;
        textMesh.alignment = TextAlignmentOptions.Center;

        // 1. Aktifkan fitur gradient pada TextMeshPro
        textMesh.enableVertexGradient = true;

        // 2. Tentukan warna awal (Contoh: Kuning Terang di atas, Oranye Kemerahan di bawah)
        topColor = new Color(1f, 1f, 0.2f, 1f);     // Kuning terang
        bottomColor = new Color(1f, 0.4f, 0f, 1f);  // Oranye

        // Terapkan warna awal ke gradient teks
        ApplyGradientColors();
    }

    void Update()
    {
        // Melayang ke atas di dunia 3D
        transform.position += new Vector3(0, 1.5f * Time.deltaTime, 0);

        // Fade out dan hancur
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float fadeSpeed = 3f;

            // Kurangi nilai alpha (transparansi) pada kedua warna
            topColor.a -= fadeSpeed * Time.deltaTime;
            bottomColor.a -= fadeSpeed * Time.deltaTime;

            // Perbarui warna gradient dengan alpha yang terus berkurang
            ApplyGradientColors();

            if (topColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // Fungsi pembantu untuk menerapkan warna gradient ke TextMeshPro
    void ApplyGradientColors()
    {
        VertexGradient gradient = new VertexGradient(
            topColor,    // Top Left
            topColor,    // Top Right
            bottomColor, // Bottom Left
            bottomColor  // Bottom Right
        );
        textMesh.colorGradient = gradient;
    }
}