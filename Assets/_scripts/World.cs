using System.Collections.Generic;

public class World
{
    public int width;
    public int height;
    public Dictionary<HexCoordinates, HexTile> tiles = new();
    public List<HexChunk> chunks = new List<HexChunk>();
    public World(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void AddTile(HexTile tile)
    {
        tiles[tile.coordinates] = tile;
    }

    public HexTile GetTile(HexCoordinates coords)
    {
        coords = Wrap(coords);

        tiles.TryGetValue(coords, out HexTile tile);
        return tile;
    }
    public HexCoordinates Wrap(HexCoordinates coords)
    {
        int q = coords.q;
        int r = coords.r;

        q = (q % width + width) % width;

        return new HexCoordinates(q, r);
    }
}