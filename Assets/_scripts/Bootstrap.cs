using UnityEngine;

public class WorldBootstrap : MonoBehaviour
{
    public int width = 50;
    public int height = 30;
    public WorldRenderer worldRenderer;

    private World world;

    void Start()
    {
        WorldGenerator generator = new WorldGenerator();
        world = generator.Generate(width, height);
        worldRenderer.Render(world);

        Debug.Log("World generated: " + world.tiles.Count + " hexes");
    }
}