using UnityEngine;

public class Tile
{

    public int q;
    public int r;

    public float height;
    public Vector3 worldPosition;
    public Biome biome;
    public Color color;

    public Tile[] neighbors = new Tile[6];

    public Tile(int q, int r, int mapWidth)
    {
        this.q = q;
        this.r = r;
        

        biome = HeightGenerator.GetBiomeFromHeight(height);

        color = HeightGenerator.GetBiomeColor(biome);

        Vector3 pos = Metrics.GetPosition(q, r);
        pos.y = height;

        worldPosition = pos;
    }
    public void ResolveBeach()
    {
        if (height > 0.05f && height <= 0.25f)
        {
            bool nearOcean = false;

            foreach (var n in neighbors)
            {
                if (n != null && n.height < 0.01f)
                {
                    nearOcean = true;
                    break;
                }
            }

            // 1 hex od oceanu
            if (nearOcean)
            {
                height = 0.15f;
                worldPosition.y = height;

                biome = Biome.Beach;
                color = HeightGenerator.GetBiomeColor(biome);
            }
            else
            {
                // 2 hexy od oceanu (rzadko)
                foreach (var n in neighbors)
                {
                    if (n == null) continue;

                    foreach (var nn in n.neighbors)
                    {
                        if (nn != null && nn.height < 0.01f && Random.value < 0.2f)
                        {
                            height = 0.15f;
                            worldPosition.y = height;

                            biome = Biome.Beach;
                            color = HeightGenerator.GetBiomeColor(biome);
                            return;
                        }
                    }
                }
            }
        }
    }

}
