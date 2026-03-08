using UnityEngine;

public static class HexEdgeUtility
{
    public static float cliffThreshold = 0.05f;

    public static EdgeType EvaluateEdge(HexTile hex, HexTile neighbor)
    {
        if (neighbor == null)
            return EdgeType.Flat;

        float diff = hex.height - neighbor.height;

        if (diff <= 0f)
            return EdgeType.Flat;

        if (diff >= cliffThreshold)
            return EdgeType.CliffDown;

        return EdgeType.SlopeDown;
    }
}