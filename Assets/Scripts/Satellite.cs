using System.Collections;
using UnityEngine;

public class Satellite : MonoBehaviour
{
    [Header("Inscribed")]
    public MenuManager menuManager;
    public Sprite satelliteSprite;

    [Header("Timing")]
    public float minInterval = 15f;
    public float maxInterval = 35f;
    public float speed = 8f;

    [Header("Path")]
    public float spawnY = 15f;
    public float spawnZ = 5f;

    private GameObject _satellite;
    private bool _active = false;

    void Start()
    {
        StartCoroutine(SatelliteLoop());
    }

    Vector2 GetIntervalRange()
    {
        if (DifficultyManager.Instance == null) 
        {
            return new Vector2(minInterval, maxInterval);
        }

        switch (DifficultyManager.Instance.current)
        {
            case Difficulty.Easy:
            {
                return new Vector2(40f, 70f);
            }

            case Difficulty.Medium:
            {
                return new Vector2(20f, 40f);
            }

            case Difficulty.Hard:
            {
                return new Vector2(8f,  16f);
            }

            default:
            {
                return new Vector2(minInterval, maxInterval);
            }
        }
    }

    IEnumerator SatelliteLoop()
    {
        while (true)
        {
            Vector2 range = GetIntervalRange();
            float wait = Random.Range(range.x, range.y);

            yield return new WaitForSeconds(wait);

            if (_active) 
            {
                continue;
            }

            yield return StartCoroutine(FlyAcross());
        }
    }

    IEnumerator FlyAcross()
    {
        _active = true;

        _satellite = new GameObject("Satellite");

        _satellite.transform.SetParent(transform);

        SpriteRenderer sr = _satellite.AddComponent<SpriteRenderer>();

        sr.sprite = satelliteSprite != null ? satelliteSprite : CreateSatelliteSprite();

        sr.sortingOrder = -5;

        _satellite.transform.localScale = Vector3.one * 4f;

        CircleCollider2D col = _satellite.AddComponent<CircleCollider2D>();

        col.isTrigger = true;

        col.radius = 0.5f;

        SatelliteHitDetector detector = _satellite.AddComponent<SatelliteHitDetector>();

        detector.satellite = this;

        bool leftToRight = Random.value > 0.5f;
        float startX = leftToRight ? -5f : 250f;
        float endX = leftToRight ? 250f : -5f;
        float dir = leftToRight ? 1f : -1f;

        _satellite.transform.position = new Vector3(startX, spawnY + Random.Range(-5f, 5f), spawnZ);

        float wobbleFreq = Random.Range(1f, 3f);
        float wobbleAmp = Random.Range(0.3f, 1f);
        float startY = _satellite.transform.position.y;

        while (_satellite != null)
        {
            float x = _satellite.transform.position.x + dir * speed * Time.deltaTime;
            float y = startY + Mathf.Sin(Time.time * wobbleFreq) * wobbleAmp;

            _satellite.transform.position = new Vector3(x, y, spawnZ);

            _satellite.transform.Rotate(0, 0, dir * 20f * Time.deltaTime);

            if (leftToRight && x > endX) 
            {
                break;
            }

            if (!leftToRight && x < endX) 
            {
                break;
            }

            yield return null;
        }

        if (_satellite != null)
        {
            Destroy(_satellite);
        }

        _satellite = null;

        _active = false;
    }

    public void OnHit()
    {
        StopAllCoroutines();

        if (_satellite != null)
        {
            StartCoroutine(SatelliteExplosion());
        }

        if (menuManager != null)
        {
            menuManager.ShowSatelliteLoss();
        }
    }

    IEnumerator SatelliteExplosion()
    {
        if (_satellite == null) 
        {
            yield break;
        }

        SpriteRenderer sr = _satellite.GetComponent<SpriteRenderer>();
        float elapsed = 0f;

        while (elapsed < 0.5f && _satellite != null)
        {
            elapsed += Time.deltaTime;

            if (sr != null)
            {
                sr.color = Color.Lerp(Color.white, Color.clear, elapsed / 0.5f);
            }

            _satellite.transform.localScale += Vector3.one * Time.deltaTime * 3f;

            yield return null;
        }

        if (_satellite != null) 
        {
            Destroy(_satellite);
        }

        _satellite = null;

        _active = false;
    }

    Sprite CreateSatelliteSprite()
    {
        int w = 32;
        int h = 16;
        Texture2D tex = new Texture2D(w, h);

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                tex.SetPixel(x, y, Color.clear);
            }
        }

        Color bodyCol = new Color(0.7f, 0.7f, 0.8f);

        for (int x = 12; x < 20; x++)
        {
            for (int y = 5; y < 11; y++)
            {
                tex.SetPixel(x, y, bodyCol);
            }
        }

        Color panelCol = new Color(0.1f, 0.4f, 0.9f);

        for (int x = 2; x < 11; x++)
        {
            for (int y = 6; y < 10; y++)
            {
                tex.SetPixel(x, y, panelCol);
            }
        }

        for (int x = 21; x < 30; x++)
        {
            for (int y = 6; y < 10; y++)
            {
                tex.SetPixel(x, y, panelCol);
            }
        }

        Color antennaCol = new Color(0.9f, 0.9f, 0.9f);

        for (int y = 10; y < 15; y++)
        {
            tex.SetPixel(16, y, antennaCol);
        }

        Color gridCol = new Color(0f, 0.2f, 0.6f);

        for (int y = 6; y < 10; y++)
        {
            tex.SetPixel(5, y, gridCol);
            tex.SetPixel(8, y, gridCol);
            tex.SetPixel(23, y, gridCol);
            tex.SetPixel(26, y, gridCol);
        }

        tex.Apply();

        tex.filterMode = FilterMode.Point;

        return Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), 16f);
    }
}

public class SatelliteHitDetector : MonoBehaviour
{
    public Satellite satellite;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Projectile>() != null)
        {
            satellite.OnHit();
        }
    }
}