using UnityEngine;

public class InputManager : MonoBehaviour
{
    Camera cam;
    GameObject currentHighlight;
    public Material highlightMaterial;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit))
            return;

        Chunk chunk = hit.collider.GetComponent<Chunk>();

        if (chunk == null)
            return;

        Tile tile = GetTileFromPosition(chunk, hit.point);

        if (tile == null)
            return;

        DebugTile(tile, chunk);
    }

    Tile GetTileFromPosition(Chunk chunk, Vector3 point)
    {
        Tile closest = null;
        float minDist = float.MaxValue;

        foreach (Tile tile in chunk.tiles)
        {
            float dist = Vector3.Distance(point, tile.worldPosition);

            if (dist < minDist)
            {
                minDist = dist;
                closest = tile;
            }
        }

        return closest;
    }

    void DebugTile(Tile tile, Chunk chunk)
    {
        string log = $"=== TILE ===\n";

        log += $"q,r: ({tile.q}, {tile.r})\n";
        log += $"height: {tile.height}\n";
        log += $"biome: {tile.biome}\n";
        log += $"world pos: {tile.worldPosition}\n";

        log += $"chunk: {chunk.name}\n";

        // 🔥 neighbors
        log += "neighbors:\n";
        for (int i = 0; i < 6; i++)
        {
            Tile n = tile.neighbors[i];

            if (n != null)
                log += $"  {((Directions)i)} -> ({n.q},{n.r}) h:{n.height}\n";
            else
                log += $"  {((Directions)i)} -> NULL\n";
        }

        Debug.Log(log);
        HighlightTile(tile);
    }
    void HighlightTile(Tile tile)
    {
        // usuń poprzedni
        if (currentHighlight != null)
            Destroy(currentHighlight);

            currentHighlight = CreateRing(
                tile.worldPosition + Vector3.up * 0.01f,
                Metrics.radius - 0.14f,
                0.08f, // grubość ringa
                40
            );
    }
    GameObject CreateRing(Vector3 center, float outerRadius, float thickness, int segments)
    {
        GameObject obj = new GameObject("Highlight");
        obj.transform.position = center;

        MeshFilter mf = obj.AddComponent<MeshFilter>();
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();

        mr.material = highlightMaterial;

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[segments * 2];
        int[] triangles = new int[segments * 6];

        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);

            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            // outer
            vertices[i] = new Vector3(cos * outerRadius, 0f, sin * outerRadius);

            // inner
            vertices[i + segments] = new Vector3(cos * (outerRadius - thickness), 0f, sin * (outerRadius - thickness));
        }

        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;

            int outerCurrent = i;
            int outerNext = next;
            int innerCurrent = i + segments;
            int innerNext = next + segments;

            int triIndex = i * 6;

            triangles[triIndex] = outerCurrent;
            triangles[triIndex + 1] = innerNext;
            triangles[triIndex + 2] = innerCurrent;

            triangles[triIndex + 3] = outerCurrent;
            triangles[triIndex + 4] = outerNext;
            triangles[triIndex + 5] = innerNext;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        mf.mesh = mesh;

        obj.transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        return obj;
    }
}