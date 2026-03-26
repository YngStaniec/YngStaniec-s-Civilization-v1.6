using UnityEngine;

public class Grid
{
    public int width;
    public int height;

    public Tile[,] tiles;

    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;

        CreateTiles();
        AssignNeighbors();

    }
    void CreateTiles()
    {
        tiles = new Tile[width, height];

        for (int q = 0; q < width; q++)
        {
            for (int r = 0; r < height; r++)
            {
                tiles[q,r] = new Tile(q,r,width);
            }
        }
    }
    void AssignNeighbors()
    {
        for (int q = 0; q < width; q++)
        {
            for (int r = 0; r < height; r++)
            {
                Tile tile = tiles[q, r];

                SetNeighbor(tile, Directions.NE, q, r + 1);
                SetNeighbor(tile, Directions.E, WrapX(q + 1), r);
                SetNeighbor(tile, Directions.SE, WrapX(q + 1), r - 1);
                SetNeighbor(tile, Directions.SW, q, r - 1);
                SetNeighbor(tile, Directions.W, WrapX(q - 1), r);
                SetNeighbor(tile, Directions.NW, WrapX(q - 1), r + 1);

            }
        }
    }
    void SetNeighbor(Tile tile, Directions dir, int q, int r)
    {
        if (r < 0 || r >= height)
            return;

        if (q < 0 || q >= width)
            return;

        tile.neighbors[(int)dir] = tiles[q, r];
    }

    public void DebugNeighbors()
    {
        for (int q = 0; q < width; q++)
        {
            for (int r = 0; r < height; r++)
            {
                Tile tile = tiles[q, r];

                string log = $"Tile ({q},{r}) neighbors: ";

                for (int dir = 0; dir < 6; dir++)
                {
                    Tile n = tile.neighbors[dir];

                    if (n != null)
                        log += $"{(Directions)dir}->({n.q},{n.r}) ";
                    else
                        log += $"{(Directions)dir}->NULL ";
                }

                Debug.Log(log);
            }
        }
    }

    int WrapX(int q)
    {
        if (q < 0)
            return width - 1;

        if (q >= width)
            return 0;

        return q;
    }
}