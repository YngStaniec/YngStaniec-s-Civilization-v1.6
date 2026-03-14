using UnityEngine;

public enum HexDirection
{
    NE,
    E,
    SE,
    SW,
    W,
    NW
}

public class HexTile
{
    public Vector3 center;

    public Vector3[] vertices = new Vector3[6];
    public HexTile[] neighbors = new HexTile[6];

    public bool[] wallExists = new bool[6];

    float radius = 1f;

    // mapowanie kierunek -> edge hexa
    static readonly int[,] edgeVertices =
    {
        {1,2}, // NE (północny-wschód)
        {0,1}, // E  (wschód)
        {5,0}, // SE (południowy-wschód)
        {4,5}, // SW (południowy-zachód)
        {3,4}, // W  (zachód)
        {2,3}  // NW (północny-zachód)
    };


    public HexTile(Vector3 center)
    {
        this.center = center;
        GenerateVertices();
    }

    void GenerateVertices()
    {
        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (60 * i - 30);

            float x = center.x + radius * Mathf.Cos(angle);
            float z = center.z + radius * Mathf.Sin(angle);

            vertices[i] = new Vector3(x, center.y, z);
        }
    }

    public void DrawHex()
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 a = vertices[i];
            Vector3 b = vertices[(i + 1) % 6];

            Debug.DrawLine(a, b, Color.white, 100f);
        }
    }

    public void TryCreateWalls()
    {
        for (int dir = 0; dir < 6; dir++)
        {
            HexTile neighbor = neighbors[dir];

            if (neighbor == null)
                continue;

            Debug.Log(
                "HEX " + center +
                " DIR " + (HexDirection)dir +
                " NEIGHBOR " + neighbor.center
            );

            if (wallExists[dir])
                continue;

            CreateWall(dir, neighbor);

            wallExists[dir] = true;
            neighbor.wallExists[(dir + 3) % 6] = true;
        }
    }

    void CreateWall(int dir, HexTile neighbor)
    {
        int a1i = edgeVertices[dir,0];
        int a2i = edgeVertices[dir,1];

        int opposite = (dir + 3) % 6;

        int b1i = edgeVertices[opposite,0];
        int b2i = edgeVertices[opposite,1];

        Vector3 a1 = vertices[a1i];
        Vector3 a2 = vertices[a2i];

        Vector3 b1 = neighbor.vertices[b1i];
        Vector3 b2 = neighbor.vertices[b2i];

        // debug edge
        Debug.DrawLine(a1,a2,Color.green,100f);
        Debug.DrawLine(b1,b2,Color.blue,100f);

        BuildWall(a1,a2,b1,b2);
    }


    void BuildWall(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        Debug.DrawLine(a, b, Color.red, 100f);
        Debug.DrawLine(b, c, Color.red, 100f);
        Debug.DrawLine(c, d, Color.red, 100f);
        Debug.DrawLine(d, a, Color.red, 100f);
    }
}