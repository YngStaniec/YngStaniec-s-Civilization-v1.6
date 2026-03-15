using UnityEngine;

public static class Metrics
{
    public static readonly float radius = 1f;

    public static readonly float hexWidth = Mathf.Sqrt(3f) * radius;
    public static readonly float hexHeight = 2f * radius;

    public static readonly float verticalSpacing = 1.5f * radius;

    public static readonly Vector3[] corners =
    {
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * -30), 0, Mathf.Sin(Mathf.Deg2Rad * -30)) * radius,
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * 30), 0, Mathf.Sin(Mathf.Deg2Rad * 30)) * radius,
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * 90), 0, Mathf.Sin(Mathf.Deg2Rad * 90)) * radius,
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * 150), 0, Mathf.Sin(Mathf.Deg2Rad * 150)) * radius,
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * 210), 0, Mathf.Sin(Mathf.Deg2Rad * 210)) * radius,
        new Vector3(Mathf.Cos(Mathf.Deg2Rad * 270), 0, Mathf.Sin(Mathf.Deg2Rad * 270)) * radius
    };
    public static Vector3 GetPosition(int q, int r)
    {
        float x = hexWidth * (q + r * 0.5f);
        float z = verticalSpacing * r;

        return new Vector3(x, 0f, z);
    }

}
