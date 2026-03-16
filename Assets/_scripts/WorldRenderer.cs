using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    public Material terrainMaterial;
    public Material cliffMaterial;

    public int width = 32;
    public int height = 32;

    public int chunkSize = 16;

    Grid grid;

    void Start()
    {
        grid = new Grid(width, height);
        Metrics.worldWidth = width * Metrics.hexWidth;

        if (width % chunkSize != 0 || height % chunkSize != 0)
        {
            Debug.LogError("Grid size must be divisible by chunkSize!");
        }
        foreach (Tile tile in grid.tiles)
        {

            Vector3 pos = tile.worldPosition;

            tile.worldPosition = pos;
        }

        CreateChunks();
    }

    void CreateChunks()
    {
        int chunkCountX = width / chunkSize;
        int chunkCountZ = height / chunkSize;
        


        for (int cx = 0; cx < chunkCountX; cx++)
        {
            for (int cz = 0; cz < chunkCountZ; cz++)
            {
                CreateChunk(cx, cz);
            }
        }
    }
    void CreateChunk(int cx, int cz)
    {
        GameObject chunkObject = new GameObject($"Chunk_{cx}_{cz}");

        chunkObject.transform.parent = transform;
        chunkObject.transform.localPosition = Vector3.zero;

        MeshFilter filter = chunkObject.AddComponent<MeshFilter>();
        MeshRenderer renderer = chunkObject.AddComponent<MeshRenderer>();

        Chunk chunk = chunkObject.AddComponent<Chunk>();
        
        chunk.terrainMaterial = terrainMaterial;
        chunk.cliffMaterial = cliffMaterial;

        Tile[,] chunkTiles = new Tile[chunkSize, chunkSize];

        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                int q = cx * chunkSize + x;
                int r = cz * chunkSize + z;

                chunkTiles[x, z] = grid.tiles[q, r];
            }
        }

        chunk.tiles = chunkTiles;

        chunk.Build();
    }

}