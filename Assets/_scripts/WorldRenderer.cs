using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    public GameObject chunkPrefab;

    public void Render(World world)
    {
        foreach (var chunk in world.chunks)
        {
            GameObject obj = Instantiate(chunkPrefab, transform);

            HexChunkRenderer renderer = obj.GetComponent<HexChunkRenderer>();

            renderer.Build(chunk);
        }
    }
}