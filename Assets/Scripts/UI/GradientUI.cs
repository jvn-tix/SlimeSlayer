using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientUI : BaseMeshEffect
{
    [SerializeField] private Color topColor = Color.white;
    [SerializeField] private Color bottomColor = Color.black;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0) return;

        List<UIVertex> vertices = new List<UIVertex>();
        vh.GetUIVertexStream(vertices);

        float minUIY = float.MaxValue;
        float maxUIY = float.MinValue;

        for (int i = 0; i < vertices.Count; i++)
        {
            float y = vertices[i].position.y;
            if (y < minUIY) minUIY = y;
            if (y > maxUIY) maxUIY = y;
        }

        float uiHeight = maxUIY - minUIY;
        if (uiHeight <= 0) uiHeight = 0.0001f;

        for (int i = 0; i < vertices.Count; i++)
        {
            UIVertex v = vertices[i];
            float t = (v.position.y - minUIY) / uiHeight;
            v.color *= Color.Lerp(bottomColor, topColor, t);
            vertices[i] = v;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(vertices);
    }
}
