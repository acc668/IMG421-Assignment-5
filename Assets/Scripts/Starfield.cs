using System.Collections;
using UnityEngine;

public class StarField : MonoBehaviour
{
    [Header("Inscribed")]
    public int numStars = 150;
    public Vector3 minPos = new Vector3(-30, -10, 10);
    public Vector3 maxPos = new Vector3(300, 60, 15);
    [Tooltip("Min and max star scale")]
    public Vector2 scaleRange = new Vector2(0.3f, 1.2f);
    [Tooltip("Min and max star brightness")]
    public Vector2 brightnessRange = new Vector2(0.6f, 1f);
    [Tooltip("Chance a star twinkles (0-1)")]
    public float twinkleChance = 0.3f;

    void Start()
    {
        for (int i = 0; i < numStars; i++)
        {
            GameObject star = new GameObject("Star_" + i);

            star.transform.SetParent(transform);

            SpriteRenderer sr = star.AddComponent<SpriteRenderer>();

            sr.sprite = CreateCircleSprite();

            sr.sortingOrder = -10;

            float brightness = Random.Range(brightnessRange.x, brightnessRange.y);

            sr.color = new Color(brightness, brightness, brightness * 0.9f + 0.1f, 1f);

            float scale = Random.Range(scaleRange.x, scaleRange.y);

            star.transform.localScale = Vector3.one * scale;

            star.transform.position = RandomPos();

            if (Random.value < twinkleChance)
            {
                StartCoroutine(Twinkle(sr));
            }
        }
    }

    Vector3 RandomPos()
    {
        return new Vector3(
            Random.Range(minPos.x, maxPos.x),
            Random.Range(minPos.y, maxPos.y),
            Random.Range(minPos.z, maxPos.z));
    }

    Sprite CreateCircleSprite()
    {
        int size = 16;
        Texture2D tex = new Texture2D(size, size);
        float center = size / 2f;
        float radius = size / 2f;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                float alpha = Mathf.Clamp01(1f - (dist / radius));

                tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
            }
        }

        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }

    IEnumerator Twinkle(SpriteRenderer sr)
    {
        while (true)
        {
            float waitTime = Random.Range(1f, 4f);

            yield return new WaitForSeconds(waitTime);

            float duration = Random.Range(0.3f, 0.8f);
            float elapsed = 0f;
            Color baseColor = sr.color;
            float dimmed = baseColor.r * 0.3f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                float t = Mathf.PingPong(elapsed * 2f / duration, 1f);
                float brightness = Mathf.Lerp(baseColor.r, dimmed, t);

                sr.color = new Color(brightness, brightness, brightness, 1f);

                yield return null;
            }

            sr.color = baseColor;
        }
    }
}