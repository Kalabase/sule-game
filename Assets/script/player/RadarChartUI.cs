using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class RadarChartUI : Graphic
{
    public int featureCount = 3; // Kenar sayısı
    public float[] values = { 1, 1, 1 }; // 0-1 arasında değerler
    public float radius = 20f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Vector2 center = rectTransform.rect.center;
        float angleStep = 2 * Mathf.PI / featureCount;

        // Noktaları hesapla
        Vector2[] points = new Vector2[featureCount];

        for (int i = 0; i < featureCount; i++)
        {
            float value = Mathf.Clamp01(values[i]);
            float angle = i * angleStep + Mathf.Deg2Rad * 30f;
            float x = Mathf.Cos(angle) * radius * value;
            float y = Mathf.Sin(angle) * radius * value;
            points[i] = center + new Vector2(x, y);
        }

        // Üçgenleri çiz
        for (int i = 0; i < featureCount; i++)
        {
            int next = (i + 1) % featureCount;

            UIVertex v0 = UIVertex.simpleVert;
            v0.color = color;
            v0.position = center;

            UIVertex v1 = UIVertex.simpleVert;
            v1.color = color;
            v1.position = points[i];

            UIVertex v2 = UIVertex.simpleVert;
            v2.color = color;
            v2.position = points[next];

            int index = vh.currentVertCount;

            vh.AddVert(v0);
            vh.AddVert(v1);
            vh.AddVert(v2);
            vh.AddTriangle(index, index + 1, index + 2);
        }
    }

    public void SetValues(float[] newValues)
    {
        if (newValues.Length != featureCount)
        {
            Debug.LogError("Veri sayısı kenar sayısıyla eşleşmiyor.");
            return;
        }
        values = newValues;
        SetVerticesDirty(); // UI'yı güncelle
    }
}
