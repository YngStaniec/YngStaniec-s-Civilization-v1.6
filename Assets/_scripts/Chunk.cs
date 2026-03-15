using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Tile[,] tiles;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    Mesh mesh;

    public void Build()
    {
        mesh = new Mesh();
        mesh.name = "Chunk Mesh";

        GetComponent<MeshFilter>().mesh = mesh;

        vertices.Clear();
        triangles.Clear();

        foreach (Tile tile in tiles)
        {
            TriangulateTile(tile);
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
    void TriangulateTile(Tile tile)
    {
        Vector3 center = tile.worldPosition;

        for (int i = 0; i < 6; i++)
        {
            Vector3 v1 = center;
            Vector3 v2 = center + Metrics.corners[i];
            Vector3 v3 = center + Metrics.corners[(i + 1) % 6];

            AddTriangle(v1, v3, v2);
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