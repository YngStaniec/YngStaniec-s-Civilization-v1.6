using System.Collections.Generic;

public class HexChunk
{
    public enum ChunkState
    {
        Inactive,
        Active,
        Dirty
    }
    public const int chunkSize = 10;

    public List<HexTile> tiles = new List<HexTile>();

    public int chunkX;
    public int chunkZ;
    public ChunkState state = ChunkState.Active;

    public HexChunk(int x, int z)
    {
        chunkX = x;
        chunkZ = z;
    }

    public void AddTile(HexTile tile)
    {
        tiles.Add(tile);
    }
}