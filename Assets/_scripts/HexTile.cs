public class HexTile
{
    public HexCoordinates coordinates;

    public float height;

    public bool water;
    public bool lake;
    public bool ocean;

    public bool[] rivers = new bool[6];
    public bool[] cliffs = new bool[6];

    public HexTile[] neighbors = new HexTile[6];
    public HexChunk chunk;

    public HexTile(HexCoordinates coordinates)
    {
        this.coordinates = coordinates;
    }

    public void SetNeighbor(HexDirection direction, HexTile neighbor)
    {
        neighbors[(int)direction] = neighbor;
    }

    public HexTile GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }
}