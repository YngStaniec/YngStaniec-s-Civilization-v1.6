using UnityEngine;

public class HexGridTest : MonoBehaviour
{
    void Start()
    {
        float r = 1f;

        float xOffset = Mathf.Sqrt(3) * r;
        float zOffset = 1.5f * r;

        HexTile hexA = new HexTile(new Vector3(0, 0.5f, 0));
        HexTile hexB = new HexTile(new Vector3(xOffset, 0.3f, 0));
        HexTile hexC = new HexTile(new Vector3(xOffset * 0.5f, 0.2f, zOffset));

        // ustawianie sąsiadów
        hexA.neighbors[(int)HexDirection.E] = hexB;
        hexB.neighbors[(int)HexDirection.W] = hexA;

        hexA.neighbors[(int)HexDirection.NE] = hexC;
        hexC.neighbors[(int)HexDirection.SW] = hexA;

        hexB.neighbors[(int)HexDirection.NW] = hexC;
        hexC.neighbors[(int)HexDirection.SE] = hexB;

        // rysowanie hexów
        hexA.DrawHex();
        hexB.DrawHex();
        hexC.DrawHex();

        // tworzenie ścian
        hexA.TryCreateWalls();
        hexB.TryCreateWalls();
        hexC.TryCreateWalls();
    }
}