using UnityEngine;

public class WorldGenerator
{
    public float heightScale = 2f;
    [SerializeField] float noiseScale = 0.07f;
    [SerializeField] float heightMultiplier = 2f;
    [SerializeField] int seed = 12345;
    public World Generate(int width, int height)
    {
        World world = new World(width, height);

        for (int q = 0; q < width; q++)
        {
            for (int r = 0; r < height; r++)
            {
                HexCoordinates coords = new HexCoordinates(q, r);
                HexTile tile = new HexTile(coords);
                tile.height = GenerateHeight(coords.q, coords.r);

                world.AddTile(tile);
            }
        }

        BuildNeighbors(world);   // ✅ TYLKO RAZ
        BuildChunks(world, width, height);
        Debug.Log("Chunks: " + world.chunks.Count);

        return world;
    }
    float GenerateHeight(int q, int r)
    {
        float x = (q + seed) * noiseScale;
        float z = (r + seed) * noiseScale;

        float noise = Mathf.PerlinNoise(x, z);

        return noise * heightMultiplier;
    }
    void BuildNeighbors(World world)
    {
        foreach (HexTile tile in world.tiles.Values)
        {
            foreach (HexDirection direction in System.Enum.GetValues(typeof(HexDirection)))
            {
                HexCoordinates offset = direction.ToOffset(tile.coordinates.r);

                HexCoordinates neighborCoords =
                    tile.coordinates + offset;

                HexTile neighbor = world.GetTile(neighborCoords);

                if (neighbor != null)
                {
                    tile.SetNeighbor(direction, neighbor);
                }
                if (tile.coordinates.q == world.width - 1 && direction == HexDirection.E)
                {
                    Debug.Log("Wrap test: " + tile.coordinates + " -> " + neighbor.coordinates);
                }
            }

        }

    }
    void BuildChunks(World world, int width, int height)
    {
        int chunkSize = HexChunk.chunkSize;

        int chunkCountX = Mathf.CeilToInt((float)width / chunkSize);
        int chunkCountZ = Mathf.CeilToInt((float)height / chunkSize);

        for (int cx = 0; cx < chunkCountX; cx++)
        {
            for (int cz = 0; cz < chunkCountZ; cz++)
            {
                HexChunk chunk = new HexChunk(cx, cz);

                for (int x = 0; x < chunkSize; x++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        int worldX = cx * chunkSize + x;
                        int worldZ = cz * chunkSize + z;

                        HexCoordinates coords = new HexCoordinates(worldX, worldZ);

                        if (world.tiles.TryGetValue(coords, out HexTile tile))
                        {
                            chunk.AddTile(tile);
                            tile.chunk = chunk;
                        }
                    }
                }

                world.chunks.Add(chunk);
            }
        }
    }
}