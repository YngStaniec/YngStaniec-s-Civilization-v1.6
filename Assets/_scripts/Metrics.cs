using UnityEngine;

public static class Metrics
{
    public const float radius = 1f;
    public static readonly float hexWidth = Mathf.Sqrt(3f) * radius;
    public static readonly float hexHeight = 2f * radius;

    public static readonly float verticalSpacing = 1.5f * radius;
    public static float worldWidth;


    public static readonly Vector3[] corners =
    {
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * -30), 0, Mathf.Sin(Mathf.Deg2Rad * -30)) * radius,
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * 30), 0, Mathf.Sin(Mathf.Deg2Rad * 30)) * radius,
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * 90), 0, Mathf.Sin(Mathf.Deg2Rad * 90)) * radius,
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * 150), 0, Mathf.Sin(Mathf.Deg2Rad * 150)) * radius,
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * 210), 0, Mathf.Sin(Mathf.Deg2Rad * 210)) * radius,
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * 270), 0, Mathf.Sin(Mathf.Deg2Rad * 270)) * radius
    };
    public static Vector3 GetCorner(Vector3 center, int i)
    {
        return center + corners[i];
    }
    public static Vector3 GetPosition(int q, int r)
    {
        float x = hexWidth * (q + r * 0.5f);
        float z = verticalSpacing * r;

        return new Vector3(x, 0f, z);
    }

    public static readonly int[,] edgeVertices =
    {
        {1,2}, // NE
        {0,1}, // E
        {5,0}, // SE
        {4,5}, // SW
        {3,4}, // W
        {2,3}  // NW
    };
    public static Vector3 GetCorner(int index)
    {
        return corners[index];
    }


}
