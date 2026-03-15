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

        height = Random.Range(0f, 1f);

        Vector3 pos = Metrics.GetPosition(q, r);
        pos.y = height;

        worldPosition = pos;
    }

}
