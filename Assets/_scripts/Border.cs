using System.Collections.Generic;
using UnityEngine;

public static class HexEdgeLines
{
    static float lineWidth = 0.02f;
    static float yOffset = 0.005f;
    //static Dictionary<Vector2, List<float>> vertexHeights = new Dictionary<Vector2, List<float>>();

    static Vector3 GetEdgeVertex(Tile tile, int vertexIndex)
    {
        return tile.worldPosition + Metrics.corners[vertexIndex];
    }

    static void GetEdge(Tile tile, Directions dir, out Vector3 a, out Vector3 b)
    {
        int i1 = Metrics.edgeVertices[(int)dir, 0];
        int i2 = Metrics.edgeVertices[(int)dir, 1];

        a = GetEdgeVertex(tile, i1);
        b = GetEdgeVertex(tile, i2);
    }

    public static void AddEdgeLines(
        Tile tile,
        List<Vector3> vertices,
        List<int> triangles,
        List<Color> colors
    )
    {
        for (int dir = 0; dir < 6; dir++)
        {
            Tile neighbor = tile.neighbors[dir];

            if (neighbor != null)
            {
                // tylko wyższy rysuje
                if (tile.height < neighbor.height)
                    continue;

                // przy równych → tylko jeden
                if (Mathf.Abs(tile.height - neighbor.height) < 0.001f)
                {
                    int id = tile.q * 10000 + tile.r;
                    int nid = neighbor.q * 10000 + neighbor.r;

                    if (id < nid)
                        continue;
                }
            }

            // 🔥 KLUCZ: używamy tej samej krawędzi co wall
            Vector3 a, b;
            GetEdge(tile, (Directions)dir, out a, out b);

            // 🔥 wysokość = najwyższy
            float y = tile.height;
            if (neighbor != null)
                y = Mathf.Max(tile.height, neighbor.height);

            a.y = y + 0.02f;
            b.y = y + 0.02f;

            AddLine(a, b, vertices, triangles, colors);
        }
    }


    static void AddLine(
        Vector3 a,
        Vector3 b,
        List<Vector3> vertices,
        List<int> triangles,
        List<Color> colors
    )
    {
        Vector3 dir = (b - a).normalized;
        Vector3 perp = new Vector3(-dir.z, 0, dir.x) * lineWidth;

        int index = vertices.Count;

        vertices.Add(a - perp + Vector3.up * yOffset);
        vertices.Add(a + perp + Vector3.up * yOffset);
        vertices.Add(b + perp + Vector3.up * yOffset);
        vertices.Add(b - perp + Vector3.up * yOffset);

        triangles.Add(index);
        triangles.Add(index + 1);
        triangles.Add(index + 2);

        triangles.Add(index);
        triangles.Add(index + 2);
        triangles.Add(index + 3);

        Color black = Color.black;

        colors.Add(black);
        colors.Add(black);
        colors.Add(black);
        colors.Add(black);
    }

    /*public static void CollectVertexHeights(Tile tile)
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 v = tile.worldPosition + Metrics.corners[i];

            Vector2 key = new Vector2(
                Mathf.Round(v.x * 10f) / 10f,
                Mathf.Round(v.z * 10f) / 10f
            );

            if (!vertexHeights.ContainsKey(key))
                vertexHeights[key] = new List<float>();

            vertexHeights[key].Add(tile.height);
        }
    }
    public static void BuildVerticalLines(
        List<Vector3> vertices,
        List<int> triangles,
        List<Color> colors
    )
    {
        foreach (var pair in vertexHeights)
        {
            List<float> heights = pair.Value;

            if (heights.Count < 1)
                continue;

            float minH = Mathf.Min(heights.ToArray());
            float maxH = Mathf.Max(heights.ToArray());

            if (Mathf.Abs(maxH - minH) < 0.001f)
                continue;

            Vector2 key = pair.Key;

            Vector3 bottom = new Vector3(key.x, minH, key.y);
            Vector3 top = new Vector3(key.x, maxH, key.y);

            AddVerticalLine(bottom, top, vertices, triangles, colors);
        }

        vertexHeights.Clear();
    }
    static void AddVerticalLine(
        Vector3 a,
        Vector3 b,
        List<Vector3> vertices,
        List<int> triangles,
        List<Color> colors
    )
    {
        float width = 0.03f;

        Vector3 perp = Vector3.right * width;
        Vector3 offset = Vector3.up * 0.01f;

        int index = vertices.Count;

        vertices.Add(a - perp + offset);
        vertices.Add(a + perp + offset);
        vertices.Add(b + perp + offset);
        vertices.Add(b - perp + offset);

        triangles.Add(index);
        triangles.Add(index + 1);
        triangles.Add(index + 2);

        triangles.Add(index);
        triangles.Add(index + 2);
        triangles.Add(index + 3);

        Color black = Color.black;

        colors.Add(black);
        colors.Add(black);
        colors.Add(black);
        colors.Add(black);
    }*/
}