using UnityEngine;

public class HexPrototypeGenerator : MonoBehaviour
{
    void Start()
    {
        Generate();
    }

    void Generate()
    {
        Vector3 centerPos = Vector3.zero;
        float centerHeight = Random.Range(0f,2f);

        CreateHex(centerPos, centerHeight);

        Vector3[] positions = new Vector3[7];
        float[] heights = new float[7];

        positions[0] = centerPos;
        heights[0] = centerHeight;

        // tworzymy 6 hexów wokół
        for(int i=0;i<6;i++)
        {
            positions[i+1] = centerPos + HexMetrics.neighborOffsets[i];
            heights[i+1] = Random.Range(0f,2f);

            CreateHex(positions[i+1], heights[i+1]);

            int dir = HexMetrics.GetDirection(centerPos, positions[i+1]);

            HexWallGenerator.CreateWall(
                centerPos,
                centerHeight,
                positions[i+1],
                heights[i+1],
                dir
            );
        }

        // ściany między hexami w pierścieniu
        for(int i=0;i<6;i++)
        {
            int next = (i+1)%6;

            int dir = HexMetrics.GetDirection(
                positions[i+1],
                positions[next+1]
            );

            HexWallGenerator.CreateWall(
                positions[i+1],
                heights[i+1],
                positions[next+1],
                heights[next+1],
                dir
            );
        }
    }

    GameObject CreateHex(Vector3 pos, float height)
    {
        GameObject hex = new GameObject("Hex");

        MeshFilter mf = hex.AddComponent<MeshFilter>();
        MeshRenderer mr = hex.AddComponent<MeshRenderer>();

        mf.mesh = HexMeshGenerator.CreateHex();

        mr.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mr.material.color = Color.green;

        hex.transform.position = new Vector3(
            pos.x,
            height,
            pos.z
        );

        return hex;
    }
}