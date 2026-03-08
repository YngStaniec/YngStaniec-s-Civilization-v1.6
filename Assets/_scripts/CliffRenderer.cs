using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CliffRenderer : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    public void Build(HashSet<HexTile> tiles)
    {
        vertices.Clear();
        triangles.Clear();

        foreach (var tile in tiles)
        {
            TriangulateCliffs(tile);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    void TriangulateCliffs(HexTile tile)
    {
        Vector3 center = tile.coordinates.ToPosition();

        for (int i = 0; i < 6; i++)
        {
            HexDirection direction = (HexDirection)i;
            HexTile neighbor = tile.GetNeighbor(direction);

            if (neighbor == null)
                continue;

            if (tile.height == neighbor.height)
                continue;

            float top = tile.height;
            float bottom = tile.height;

            HexTile n = tile.GetNeighbor(direction);

            while (n != null && n.height < bottom)
            {
                bottom = n.height;
                n = n.GetNeighbor(direction);
            }

            if (top == bottom)
                return;

            Vector3 v1 = center + HexMetrics.GetFirstCorner(direction);
            Vector3 v2 = center + HexMetrics.GetSecondCorner(direction);

            Vector3 v3 = v1;
            Vector3 v4 = v2;

            v1.y = top;
            v2.y = top;

            v3.y = bottom;
            v4.y = bottom;

            AddQuad(v1, v2, v3, v4);
        }
    }
    float GetLowestHeight(HexTile tile, HexDirection dir)
    {
        float lowest = tile.height;

        HexTile n1 = tile.GetNeighbor(dir);
        if (n1 != null)
            lowest = Mathf.Min(lowest, n1.height);

        HexDirection nextDir = (HexDirection)(((int)dir + 1) % 6);

        HexTile n2 = tile.GetNeighbor(nextDir);
        if (n2 != null)
            lowest = Mathf.Min(lowest, n2.height);

        return lowest;
    }

    void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
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