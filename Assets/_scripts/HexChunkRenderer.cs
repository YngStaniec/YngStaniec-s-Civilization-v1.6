using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexChunkRenderer : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    BorderRenderer borderRenderer;

    public void Build(HexChunk chunk)
    {
        vertices.Clear();
        triangles.Clear();
        borderRenderer = new BorderRenderer(vertices, triangles);

        foreach (var tile in chunk.tiles)
        {
            foreach (HexDirection dir in System.Enum.GetValues(typeof(HexDirection)))
            {
                HexTile neighbor = tile.GetNeighbor(dir);

                if (neighbor == null) continue;

                EdgeType edge = HexEdgeUtility.EvaluateEdge(tile, neighbor);
                
                if (edge != EdgeType.Flat)
                {
                    Debug.DrawLine(
                        tile.coordinates.ToPosition(),
                        neighbor.coordinates.ToPosition(),
                        Color.yellow,
                        100f
                    );

                    Debug.Log("edge detected");
                }

                if (edge == EdgeType.CliffDown)
                {
                    Debug.DrawLine(
                        tile.coordinates.ToPosition(),
                        neighbor.coordinates.ToPosition(),
                        Color.red,
                        100f
                    );
                    Debug.Log("redline generated");
                }
            }

            Triangulate(tile);
            borderRenderer.Generate(tile);
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