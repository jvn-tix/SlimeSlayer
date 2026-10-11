using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    private float disappearTimer = 0.8f;

    public void Setup(int damageAmount)
    {
        if (textMesh == null) textMesh = GetComponentInChildren<TextMeshProUGUI>();

        textMesh.text = damageAmount.ToString();
        textMesh.fontSize = 36;
        textMesh.alignment = TextAlignmentOptions.Center;

        // Pastikan fitur gradient aktif (warnanya ditarik dari Inspector TMP)
        textMesh.enableVertexGradient = true;
    }

    void Update()
    {
        // Melayang ke atas
        transform.position += new Vector3(0, 1.5f * Time.deltaTime, 0);

        // Fade out dan hancur
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float fadeSpeed = 3f;

            // Ambil warna gradasi saat ini, kurangi alpha-nya secara bertahap
            VertexGradient currentGradient = textMesh.colorGradient;

            Color topLeft = currentGradient.topLeft;
            Color topRight = currentGradient.topRight;
            Color bottomLeft = currentGradient.bottomLeft;
            Color bottomRight = currentGradient.bottomRight;

            float alphaChange = fadeSpeed * Time.deltaTime;
            topLeft.a -= alphaChange;
            topRight.a -= alphaChange;
            bottomLeft.a -= alphaChange;
            bottomRight.a -= alphaChange;

            // Masukkan kembali warna yang sudah transparan
            textMesh.colorGradient = new VertexGradient(topLeft, topRight, bottomLeft, bottomRight);

            if (topLeft.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}