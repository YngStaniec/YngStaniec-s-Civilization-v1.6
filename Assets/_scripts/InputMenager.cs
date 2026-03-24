using UnityEngine;

public class InputManager : MonoBehaviour
{
    Camera cam;

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
    }
}