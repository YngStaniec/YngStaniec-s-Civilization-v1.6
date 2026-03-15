using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Grid grid = new Grid(5,5);
        grid.DebugNeighbors();

        foreach (var tile in grid.tiles)
        {
            Vector3 p = tile.worldPosition;

            Debug.DrawLine(p, p + Vector3.up * 2, Color.yellow, 100f);
        }
    }
}
