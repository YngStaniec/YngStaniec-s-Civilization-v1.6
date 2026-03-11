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

    public static readonly Vector3[] neighborOffsets =
    {
        new Vector3(hexWidth,0,0),
        new Vector3(hexWidth*0.5f,0,radius*1.5f),
        new Vector3(-hexWidth*0.5f,0,radius*1.5f),
        new Vector3(-hexWidth,0,0),
        new Vector3(-hexWidth*0.5f,0,-radius*1.5f),
        new Vector3(hexWidth*0.5f,0,-radius*1.5f)
    };

    public static Vector3 GetFirstCorner(int direction)
    {
        return corners[direction];
    }

    public static Vector3 GetSecondCorner(int direction)
    {
        return corners[(direction + 1) % 6];
    }

    public static int GetDirection(Vector3 from, Vector3 to)
    {
        Vector3 dir = (to - from).normalized;

        float bestDot = -999f;
        int bestIndex = 0;

        for(int i=0;i<6;i++)
        {
            Vector3 n = neighborOffsets[i].normalized;
            float dot = Vector3.Dot(dir, n);

            if(dot > bestDot)
            {
                bestDot = dot;
                bestIndex = i;
            }
        }

        return bestIndex;
    }
}