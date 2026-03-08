using UnityEngine;

public class WorldGenerator
{
    public float heightScale = 2f;
    public World Generate(int width, int height)
    {
        World world = new World(width, height);

        for (int q = 0; q < width; q++)
        {
            for (int r = 0; r < height; r++)
            {
                HexCoordinates coords = new HexCoordinates(q, r);
                HexTile tile = new HexTile(coords);
                tile.height = Random.Range(0f, heightScale);

                world.AddTile(tile);
            }
        }

        BuildNeighbors(world);   // ✅ TYLKO RAZ
        BuildChunks(world, width, height);
        Debug.Log("Chunks: " + world.chunks.Count);

        return world;
    }
    void BuildNeighbors(World world)
    {
        foreach (HexTile tile in world.tiles.Values)
        {
            foreach (HexDirection direction in System.Enum.GetValues(typeof(HexDirection)))
            {
                HexCoordinates neighborCoords =
                    tile.coordinates + direction.ToOffset();

                HexTile neighbor = world.GetTile(neighborCoords);

                if (neighbor != null)
                {
                    tile.SetNeighbor(direction, neighbor);
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