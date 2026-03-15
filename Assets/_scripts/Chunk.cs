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
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "Chunk Mesh";
            GetComponent<MeshFilter>().mesh = mesh;
        }

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
        mesh.RecalculateBounds();
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

        BuildWall(tile, Directions.NE);
        BuildWall(tile, Directions.E);
        BuildWall(tile, Directions.SE);

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
    Vector3 GetEdgeVertex(Tile tile, int vertexIndex)
    {
        return tile.worldPosition + Metrics.corners[vertexIndex];
    }

    void GetEdge(Tile tile, Directions dir, out Vector3 a, out Vector3 b)
    {
        int i1 = Metrics.edgeVertices[(int)dir, 0];
        int i2 = Metrics.edgeVertices[(int)dir, 1];

        a = GetEdgeVertex(tile, i1);
        b = GetEdgeVertex(tile, i2);
    }
    void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        int index = vertices.Count;

        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);
        vertices.Add(d);

        triangles.Add(index);
        triangles.Add(index + 1);
        triangles.Add(index + 2);

        triangles.Add(index);
        triangles.Add(index + 2);
        triangles.Add(index + 3);
    }
    void BuildWall(Tile tile, Directions dir)
    {
        Tile neighbor = tile.neighbors[(int)dir];

        if (neighbor == null)
            return;

        Vector3 a1, a2;
        GetEdge(tile, dir, out a1, out a2);

        int opposite = ((int)dir + 3) % 6;

        Vector3 b1, b2;
        GetEdge(neighbor, (Directions)opposite, out b1, out b2);

        AddQuad(a1, a2, b1, b2);
    }


}