using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class LunarGround : MonoBehaviour
{
    [Header("Inscribed")]
    public int textureWidth  = 512;
    public int textureHeight = 64;
    public int numCraters = 18;
    public Color baseColor = new Color(0.45f, 0.43f, 0.42f);
    public Color darkColor = new Color(0.25f, 0.24f, 0.23f);
    public Color lightColor = new Color(0.65f, 0.63f, 0.61f);

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();

        rend.material.mainTexture = GenerateTexture();

        rend.material.mainTextureScale = new Vector2(8f, 1f);
    }

    Texture2D GenerateTexture()
    {
        Texture2D tex = new Texture2D(textureWidth, textureHeight);

        tex.filterMode = FilterMode.Bilinear;

        tex.wrapMode = TextureWrapMode.Repeat;

        Color[] pixels = new Color[textureWidth * textureHeight];

        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                float noise = Mathf.PerlinNoise(x * 0.05f, y * 0.05f);
                float noise2 = Mathf.PerlinNoise(x * 0.12f + 100f, y * 0.12f);
                float combined = (noise * 0.6f) + (noise2 * 0.4f);

                pixels[y * textureWidth + x] = Color.Lerp(darkColor, lightColor, combined);
            }
        }

        for (int i = 0; i < numCraters; i++)
        {
            int cx = Random.Range(0, textureWidth);
            int cy = Random.Range(0, textureHeight);
            int radius = Random.Range(4, 18);

            for (int x = cx - radius; x < cx + radius; x++)
            {
                for (int y = cy - radius; y < cy + radius; y++)
                {
                    int px = (x + textureWidth) % textureWidth;
                    int py = Mathf.Clamp(y, 0, textureHeight - 1);
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(cx, cy));

                    if (dist < radius)
                    {
                        int idx = py * textureWidth + px;
                        float t = dist / radius;

                        if (t < 0.6f)
                        {
                            float darken = Mathf.Lerp(0.4f, 0.8f, t / 0.6f);

                            pixels[idx] = Color.Lerp(darkColor * 0.7f, pixels[idx], darken);
                        }
                        
                        else
                        {
                            float lighten = Mathf.Lerp(1f, 0f, (t - 0.6f) / 0.4f);

                            pixels[idx] = Color.Lerp(pixels[idx], lightColor, lighten * 0.5f);
                        }
                    }
                }
            }
        }

        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = textureHeight - 4; y < textureHeight; y++)
            {
                int idx = y * textureWidth + x;
                float t = (float)(y - (textureHeight - 4)) / 4f;

                pixels[idx] = Color.Lerp(pixels[idx], lightColor, t * 0.4f);
            }
        }

        tex.SetPixels(pixels);

        tex.Apply();

        return tex;
    }
}