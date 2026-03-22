using UnityEngine;

public static class HeightGenerator
{
    // ========================
    // PARAMETRY
    // ========================

    // 1. Base noise (kształt terenu)
    static float baseScale = 0.15f;
    
    // 2. Ocean noise
    static float oceanScale = 0.06f;
    static float oceanThreshold = 0.3f; // ile mapy to ocean

    // 3. Lake noise
    static float lakeScale = 0.2f;
    static float lakeChance = 0.08f;

    // 4. Mountain noise
    static float mountainScale = 0.1f;
    static float mountainThreshold = 0.75f;

    // 5. Cliff noise
    static float cliffStrength = 0.4f;
    static float cliffThreshold = 0.15f;


    // ========================
    // PUBLIC API
    // ========================

    public static float GetHeight(int q, int r, int mapWidth)
    {
        Vector3 pos = Metrics.GetPosition(q, r);
        float x = pos.x;
        float z = pos.z;

        // ========================
        // OCEANY (NAJWAŻNIEJSZE)
        // ========================
        float oceanNoise = Mathf.PerlinNoise(x * oceanScale, z * oceanScale);

        // lekka modulacja zamiast falloffa
        float continentMask = Mathf.PerlinNoise(x * 0.01f, z * 0.01f);
        oceanNoise *= Mathf.Lerp(0.5f, 1.5f, continentMask);

        float coastBlend = Mathf.InverseLerp(oceanThreshold - 0.05f, oceanThreshold + 0.05f, oceanNoise);

        if (oceanNoise < oceanThreshold - 0.05f)
        {
            return 0f; // głęboki ocean
        }

        // ========================
        // BASE TERRAIN
        // ========================
        float largeNoise = Mathf.PerlinNoise(x * 0.04f, z * 0.04f); // kształt kontynentów
        float smallNoise = Mathf.PerlinNoise(x * 0.12f, z * 0.12f); // detale

        float baseNoise = Mathf.Lerp(largeNoise, smallNoise, 0.4f);

        float height = baseNoise;

        height *= coastBlend;

        // ========================
        // GÓRY (nadpisują wszystko oprócz oceanu)
        // ========================
        float mountainNoise = Mathf.PerlinNoise(x * mountainScale + 100, z * mountainScale + 100);
        float mountainMask = Mathf.PerlinNoise(x * 0.02f + 2000, z * 0.02f + 2000);
        // ridge noise = pasma zamiast plam
        mountainNoise = 1f - Mathf.Abs(mountainNoise * 2f - 1f);

        if (height > 0.45f)
        {
            if (mountainNoise > mountainThreshold && mountainMask > 0.6f)
            {
                float m = (mountainNoise - mountainThreshold) * 3f;
                height = Mathf.Lerp(0.8f, 1f, m);
            }
        }
        else
        {
            // małe, rzadkie góry na nizinach
            float small = Mathf.PerlinNoise((x + 3000) * 0.15f, (z + 3000) * 0.15f);

            if (small > 0.85f)
            {
                height = Mathf.Lerp(0.75f, 0.9f, small);
            }
        }
        // ========================
        // KWANTYZACJA (0.05)
        // ========================
        height = Quantize(height);

        return height;
    }

    public static Biome GetBiomeFromHeight(float height)
    {
        if (height < 0.01f) return Biome.Ocean;
        if (height < 0.2f) return Biome.Plains;
        if (height < 0.5f) return Biome.Grass;
        if (height < 0.8f) return Biome.Hills;
        return Biome.Mountains;
    }

    public static Color GetBiomeColor(Biome biome)
    {
        switch (biome)
        {
            case Biome.Ocean: return new Color(0.1f, 0.3f, 0.8f);
            case Biome.Plains: return new Color(0.56f, 0.93f, 0.56f);
            case Biome.Beach: return new Color(0.9f, 0.85f, 0.6f);
            case Biome.Grass: return new Color(0.2f, 0.7f, 0.2f);
            case Biome.Hills: return new Color(0.4f, 0.6f, 0.2f);
            case Biome.Mountains: return Color.gray;
            case Biome.Lake: return new Color(0.05f, 0.1f, 0.3f);
        }

        return Color.magenta;
    }

    // ========================
    // JEZIORA (wołaj PO stworzeniu mapy!)
    // ========================
    public static void ApplyLakes(Tile[,] tiles, int width, int height)
    {
        for (int q = 0; q < width; q++)
        {
            for (int r = 0; r < height; r++)
            {
                Tile t = tiles[q, r];

                if (t.height < 0.01f) continue; // nie na oceanie

                float lakeNoise = Mathf.PerlinNoise(q * lakeScale + 200, r * lakeScale + 200);

                if (lakeNoise > 1f - lakeChance)
                {
                    float avg = 0f;
                    int count = 0;

                    foreach (var n in t.neighbors)
                    {
                        if (n != null && n.height > 0f)
                        {
                            avg += n.height;
                            count++;
                        }
                    }

                    if (count > 0)
                    {
                        float lakeHeight = avg / count - 0.05f;
                        t.height = Quantize(Mathf.Clamp01(lakeHeight));
                        t.worldPosition.y = t.height;
                        t.biome = Biome.Lake;
                        t.color = GetBiomeColor(Biome.Lake);
                    }
                }
            }
        }
    }

    // ========================
    // HELPERY
    // ========================

    static float Quantize(float h)
    {
        h = Mathf.Clamp01(h);
        h = Mathf.Round(h / 0.05f) * 0.05f;
        return Mathf.Round(h * 100f) / 100f;
    }
}