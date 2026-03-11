using UnityEngine;

public static class HexWallGenerator
{
    public static GameObject CreateWall(
        Vector3 hexApos,
        float heightA,
        Vector3 hexBpos,
        float heightB,
        int direction) // direction już nie jest używany
    {
        Vector3 center = (hexApos + hexBpos) * 0.5f;

        Vector3 dir = (hexBpos - hexApos).normalized;

        Vector3 edgeDir = Vector3.Cross(dir, Vector3.up).normalized;

        float halfEdge = HexMetrics.radius * 0.5f;

        Vector3 corner1 = center + edgeDir * halfEdge;
        Vector3 corner2 = center - edgeDir * halfEdge;

        float lower = Mathf.Min(heightA, heightB);
        float higher = Mathf.Max(heightA, heightB);

        Vector3 bottom1 = new Vector3(corner1.x, lower, corner1.z);
        Vector3 bottom2 = new Vector3(corner2.x, lower, corner2.z);

        Vector3 top1 = new Vector3(corner1.x, higher, corner1.z);
        Vector3 top2 = new Vector3(corner2.x, higher, corner2.z);

        GameObject wall = new GameObject("Wall");

        MeshFilter mf = wall.AddComponent<MeshFilter>();
        MeshRenderer mr = wall.AddComponent<MeshRenderer>();

        mr.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mr.material.color = new Color(0.4f,0.25f,0.1f);

        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[]
        {
            bottom1,
            bottom2,
            top1,
            top2
        };

        int[] triangles;

        if(heightA < heightB)
        {
            triangles = new int[]
            {
                0,2,1,
                1,2,3
            };
        }
        else
        {
            triangles = new int[]
            {
                0,1,2,
                1,3,2
            };
        }

        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        mf.mesh = mesh;

        return wall;
    }
}