using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    private float disappearTimer = 0.8f;
    private Color textColor;

    public void Setup(int damageAmount)
    {
        if (textMesh == null) textMesh = GetComponentInChildren<TextMeshProUGUI>();

        textMesh.text = damageAmount.ToString();
        textMesh.fontSize = 36;
        textMesh.color = Color.yellow;
        textMesh.alignment = TextAlignmentOptions.Center;

        textColor = textMesh.color;
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
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}