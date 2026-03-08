using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexChunkRenderer : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    public void Build(HexChunk chunk)
    {
        vertices.Clear();
        triangles.Clear();

        foreach (var tile in chunk.tiles)
        {
            Triangulate(tile);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Triangulate(HexTile tile)
    {
        Vector3 center = tile.coordinates.ToPosition();
        center.y = tile.height;

        for (int i = 0; i < 6; i++)
        {
            HexDirection direction = (HexDirection)i;

            Vector3 v1 = center + HexMetrics.GetFirstCorner(direction);
            Vector3 v2 = center + HexMetrics.GetSecondCorner(direction);

            v1.y = tile.height;
            v2.y = tile.height;

            AddTriangle(center, v1, v2);
        }
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int index = vertices.Count;

        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);

        triangles.Add(index);
        triangles.Add(index + 1);
        triangles.Add(index + 2);
    }
}