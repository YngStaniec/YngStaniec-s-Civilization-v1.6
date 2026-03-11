using UnityEngine;


public static class HexMetrics
{
    public const float radius = 1f;
    public const float hexWidth = 1.7320508f;
    
    public static readonly Vector3[] corners =
    {
        new Vector3(0,0, radius),
        new Vector3(0.8660254f,0, 0.5f),
        new Vector3(0.8660254f,0,-0.5f),
        new Vector3(0,0,-radius),
        new Vector3(-0.8660254f,0,-0.5f),
        new Vector3(-0.8660254f,0,0.5f)
    };

    static Vector3 Corner(int i)
    {
        float angle = Mathf.Deg2Rad * (60f * i + 30f);

        return new Vector3(
            radius * Mathf.Cos(angle),
            0,
            radius * Mathf.Sin(angle)
        );
    }

    public static Vector3 GetFirstCorner(int direction)
    {
        return corners[direction];
    }

    public static Vector3 GetSecondCorner(int direction)
    {
        return corners[direction + 1];
    }
}