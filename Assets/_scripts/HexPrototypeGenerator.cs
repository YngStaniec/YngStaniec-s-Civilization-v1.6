using UnityEngine;

public class HexPrototypeGenerator : MonoBehaviour
{
    void Start()
    {
        Generate();
    }

    void Generate()
    {
        float hA = Random.Range(0f, 2f);
        float hB = Random.Range(0f, 2f);

        Vector3 posA = Vector3.zero;

        Vector3 posB = new Vector3(
            HexMetrics.hexWidth,
            0,
            0
        );

        CreateHex(posA, hA);
        CreateHex(posB, hB);

        HexWallGenerator.CreateWall(
            posA,
            hA,
            posB,
            hB,
            0
        );
    }

    GameObject CreateHex(Vector3 pos, float height)
    {
        GameObject hex = new GameObject("Hex");

        MeshFilter mf = hex.AddComponent<MeshFilter>();
        MeshRenderer mr = hex.AddComponent<MeshRenderer>();

        mf.mesh = HexMeshGenerator.CreateHex();

        hex.transform.position = new Vector3(
            pos.x,
            height,
            pos.z
        );

        return hex;
    }
}