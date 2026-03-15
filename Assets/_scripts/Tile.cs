using UnityEngine;

public class Tile
{
    
    public int q;
    public int r;

    public float height;
    public Vector3 worldPosition;

    public Tile[] neighbors = new Tile[6];

    public Tile(int q, int r)
    {
        this.q = q;
        this.r = r;

        worldPosition = Metrics.GetPosition(q, r);
    }
}
