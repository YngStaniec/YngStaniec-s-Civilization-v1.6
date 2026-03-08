using System.Collections.Generic;
using UnityEngine;

public class BorderRenderer
{
    List<Vector3> vertices;
    List<int> triangles;

    public BorderRenderer(List<Vector3> v, List<int> t)
    {
        vertices = v;
        triangles = t;
    }

    public void Generate(HexTile tile)
    {
        foreach (HexDirection dir in System.Enum.GetValues(typeof(HexDirection)))
        {
            if ((int)dir > 2) continue; // tylko NE, E, SE

            HexTile neighbor = tile.GetNeighbor(dir);
            if (neighbor == null) continue;

            EdgeType edge = HexEdgeUtility.EvaluateEdge(tile, neighbor);

            if (edge == EdgeType.Flat) continue;

            CreateBorder(tile, neighbor, dir);
        }
    }

    void CreateBorder(HexTile a, HexTile b, HexDirection dir)
    {
        Vector3 center = a.coordinates.ToPosition();

        Vector3 v1 = center + HexMetrics.GetFirstCorner(dir);
        Vector3 v2 = center + HexMetrics.GetSecondCorner(dir);

        v1.y = a.height;
        v2.y = a.height;

        Vector3 v3 = v1;
        Vector3 v4 = v2;

        v3.y = b.height;
        v4.y = b.height;

        int index = vertices.Count;

        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);

        triangles.Add(index);
        triangles.Add(index + 2);
        triangles.Add(index + 1);

        triangles.Add(index + 1);
        triangles.Add(index + 2);
        triangles.Add(index + 3);
    }
}