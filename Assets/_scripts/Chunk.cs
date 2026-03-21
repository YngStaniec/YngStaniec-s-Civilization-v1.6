using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Tile[,] tiles;

    List<Vector3> vertices = new List<Vector3>();

    List<int> hexTriangles = new List<int>();
    List<int> wallTriangles = new List<int>();
    List<int> cliffTriangles = new List<int>();
    List<Color> colors = new List<Color>();

    public Material terrainMaterial;
    public Material cliffMaterial;

    Mesh mesh;

    public void Build()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "Chunk Mesh";
            GetComponent<MeshFilter>().mesh = mesh;
        }

        MeshRenderer renderer = GetComponent<MeshRenderer>();

        // 3 materiały dla 3 submeshów
        renderer.materials = new Material[]
        {
            terrainMaterial, // hex top
            terrainMaterial, // zwykłe ściany
            cliffMaterial    // klify
        };

        vertices.Clear();
        hexTriangles.Clear();
        wallTriangles.Clear();
        cliffTriangles.Clear();
        colors.Clear();

        foreach (Tile tile in tiles)
        {
            TriangulateTile(tile);
        }

        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.colors = colors.ToArray();

        mesh.subMeshCount = 3;

        mesh.SetTriangles(hexTriangles, 0);
        mesh.SetTriangles(wallTriangles, 1);
        mesh.SetTriangles(cliffTriangles, 2);

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

            AddTriangle(v1, v3, v2, tile);
        }

        BuildWall(tile, Directions.NE);
        BuildWall(tile, Directions.E);
        BuildWall(tile, Directions.SE);
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Tile tile)
    {
        int index = vertices.Count;

        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);

        colors.Add(tile.color);
        colors.Add(tile.color);
        colors.Add(tile.color);

        hexTriangles.Add(index);
        hexTriangles.Add(index + 1);
        hexTriangles.Add(index + 2);
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

    void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, bool isCliff, Color color)
    {
        int index = vertices.Count;

        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);
        vertices.Add(d);

        List<int> target = isCliff ? cliffTriangles : wallTriangles;

        target.Add(index);
        target.Add(index + 1);
        target.Add(index + 2);

        target.Add(index);
        target.Add(index + 2);
        target.Add(index + 3);

        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }

    void BuildWall(Tile tile, Directions dir)
    {
        Tile neighbor = tile.neighbors[(int)dir];

        if (neighbor == null)
            return;

        if (tile.height == neighbor.height)
            return;

        float diff = Mathf.Abs(tile.height - neighbor.height);
        Tile higher = tile.height > neighbor.height ? tile : neighbor;

        bool isCliff = diff >= 0.2f;

        Vector3 a1, a2;
        GetEdge(tile, dir, out a1, out a2);

        int opposite = ((int)dir + 3) % 6;

        Vector3 b1, b2;
        GetEdge(neighbor, (Directions)opposite, out b1, out b2);

        float dx = b1.x - a1.x;

        if (Mathf.Abs(dx) > Metrics.worldWidth * 0.5f)
        {
            if (dx > 0)
            {
                b1.x -= Metrics.worldWidth;
                b2.x -= Metrics.worldWidth;
            }
            else
            {
                b1.x += Metrics.worldWidth;
                b2.x += Metrics.worldWidth;
            }
        }

        AddQuad(a1, a2, b1, b2, isCliff, higher.color);
    }
}
