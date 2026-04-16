using System.Collections;
using UnityEngine;

public class SpaceBackground : MonoBehaviour
{
    [Header("Planets")]
    public int numPlanets = 3;
    public Vector3 planetMinPos = new Vector3(10, 5, 12);
    public Vector3 planetMaxPos = new Vector3(200, 40, 14);
    public Vector2 planetScaleRange = new Vector2(4f, 9f);

    [Header("Shooting Stars")]
    public float shootingStarMinInterval = 6f;
    public float shootingStarMaxInterval = 15f;
    public float shootingStarSpeed = 40f;

    private Color[] planetColors = new Color[]
    {
        new Color(0.4f, 0.2f, 0.8f),
        new Color(0.2f, 0.6f, 0.9f),
        new Color(0.9f, 0.4f, 0.2f),
        new Color(0.2f, 0.8f, 0.5f),
        new Color(0.8f, 0.2f, 0.4f),
    };

    void Start()
    {
        SpawnPlanets();

        StartCoroutine(ShootingStarLoop());
    }

    void SpawnPlanets()
    {
        for (int i = 0; i < numPlanets; i++)
        {
            GameObject planet = new GameObject("Planet_" + i);

            planet.transform.SetParent(transform);

            SpriteRenderer sr = planet.AddComponent<SpriteRenderer>();

            sr.sprite = CreatePlanetSprite();

            sr.sortingOrder = -8;

            Color col = planetColors[Random.Range(0, planetColors.Length)];

            sr.color = col;

            float scale = Random.Range(planetScaleRange.x, planetScaleRange.y);

            planet.transform.localScale = Vector3.one * scale;

            Vector3 pos = new Vector3(
                Random.Range(planetMinPos.x, planetMaxPos.x),
                Random.Range(planetMinPos.y, planetMaxPos.y),
                Random.Range(planetMinPos.z, planetMaxPos.z));

            planet.transform.position = pos;

            if (Random.value > 0.5f)
            {
                AddRing(planet, col);
            }
        }
    }

    void AddRing(GameObject planet, Color col)
    {
        GameObject ring = new GameObject("Ring");

        ring.transform.SetParent(planet.transform);

        ring.transform.localPosition = Vector3.zero;

        ring.transform.localRotation = Quaternion.Euler(0, 0, 30f);

        SpriteRenderer sr = ring.AddComponent<SpriteRenderer>();

        sr.sprite = CreateRingSprite();

        sr.sortingOrder = -7;

        Color ringCol = col;

        ringCol.a = 0.6f;

        sr.color = ringCol;

        ring.transform.localScale = new Vector3(2.2f, 0.6f, 1f);
    }

    Sprite CreatePlanetSprite()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size);
        
        tex.filterMode = FilterMode.Bilinear;

        float center = size / 2f;
        float radius = size / 2f;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                
                if (dist <= radius)
                {
                    float edge = Mathf.Clamp01(1f - ((dist - radius * 0.85f) / (radius * 0.15f)));
                    float shade = 1f - (dist / radius) * 0.3f;
                    float highlight = Mathf.Clamp01(1f - Vector2.Distance(new Vector2(x, y), new Vector2(center * 0.7f, center * 1.3f)) / (radius * 0.5f)) * 0.3f;

                    tex.SetPixel(x, y, new Color(shade + highlight, shade + highlight, shade + highlight, edge));
                }

                else
                {
                    tex.SetPixel(x, y, Color.clear);
                }
            }
        }

        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }

    Sprite CreateRingSprite()
    {
        int w = 128;
        int h = 32;
        Texture2D tex = new Texture2D(w, h);

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                float centerY = h / 2f;
                float t = Mathf.Abs(y - centerY) / centerY;
                float alpha = Mathf.Clamp01(1f - t * t) * 0.7f;

                tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
            }
        }

        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f));
    }

    IEnumerator ShootingStarLoop()
    {
        while (true)
        {
            float wait = Random.Range(shootingStarMinInterval, shootingStarMaxInterval);

            yield return new WaitForSeconds(wait);

            StartCoroutine(FireShootingStar());
        }
    }

    IEnumerator FireShootingStar()
    {
        GameObject star = new GameObject("ShootingStar");

        star.transform.SetParent(transform);

        SpriteRenderer sr = star.AddComponent<SpriteRenderer>();

        sr.sprite = CreateShootingStarSprite();

        sr.sortingOrder = -6;

        sr.color = new Color(1f, 1f, 0.8f, 1f);

        star.transform.localScale = new Vector3(1.5f, 0.15f, 1f);

        float startX = Random.Range(50f, 200f);
        float startY = Random.Range(20f, 50f);

        star.transform.position = new Vector3(startX, startY, 8f);

        Vector3 direction = new Vector3(-1f, -0.4f, 0f).normalized;

        star.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);

        float duration = 1.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            star.transform.position += direction * shootingStarSpeed * Time.deltaTime;

            float alpha = Mathf.Clamp01(1f - (elapsed / duration));

            sr.color = new Color(1f, 1f, 0.8f, alpha);

            yield return null;
        }

        Destroy(star);
    }

    Sprite CreateShootingStarSprite()
    {
        int w = 64;
        int h = 8;
        Texture2D tex = new Texture2D(w, h);

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                float centerY = h / 2f;
                float tY = Mathf.Abs(y - centerY) / centerY;
                float tX = (float)x / w;
                float alpha = (1f - tY * tY) * tX;

                tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
            }
        }

        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f));
    }
}