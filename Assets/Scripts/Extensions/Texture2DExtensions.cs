using UnityEngine;

internal static class Texture2DExtensions
{
    public static void DrawLine(this Texture2D tex, Vector2 p1, Vector2 p2, Color col)
    {
        Vector2 t = p1;
        float frac = 1 / Mathf.Sqrt(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2));
        float ctr = 0;

        while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y)
        {
            t = Vector2.Lerp(p1, p2, ctr);
            ctr += frac;
            tex.SetPixel((int)t.x, (int)t.y, col);
        }
    }

    public static void DrawCircle(this Texture2D tex, Vector2 p, Color color, float radius = 0.5f)
    {
        float i, angle, x1, y1;

        for (i = 0; i < 360; i += 0.1f)
        {
            angle = i;
            x1 = radius * Mathf.Cos(angle * Mathf.PI / 180);
            y1 = radius * Mathf.Sin(angle * Mathf.PI / 180);
            tex.SetPixel(Mathf.RoundToInt(p.x + x1), Mathf.RoundToInt(p.y + y1), color);
        }
    }
}
