using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S;

    [Header("SFX Clips")]
    public AudioClip launchSound;
    public AudioClip impactSound;
    public AudioClip goalSound;
    public AudioClip winSound;
    public AudioClip lossSound;

    [Header("Wind (loops on Medium/Hard)")]
    public AudioClip windSound;
    [Range(0f, 1f)]
    public float windVolume = 0.4f;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float sfxVolume = 0.8f;

    private AudioSource _source;
    private AudioSource _windSource;

    void Awake()
    {
        S = this;

        _source = GetComponent<AudioSource>();

        if (_source == null)
        {
            _source = gameObject.AddComponent<AudioSource>();
        }

        _windSource = gameObject.AddComponent<AudioSource>();

        _windSource.clip = windSound;

        _windSource.loop = true;

        _windSource.volume = windVolume;

        _windSource.playOnAwake = false;
    }

    void Start()
    {
        if (DifficultyManager.WindRange() != Vector2.zero)
        {
            START_WIND();
        }
    }

    public static void PLAY_LAUNCH()
    {
        if (S != null && S.launchSound != null)
        {
            S._source.PlayOneShot(S.launchSound, S.sfxVolume);
        }
    }

    public static void PLAY_IMPACT()
    {
        if (S != null && S.impactSound != null)
        {
            S._source.PlayOneShot(S.impactSound, S.sfxVolume * 0.6f);
        }
    }

    public static void PLAY_GOAL()
    {
        if (S != null && S.goalSound != null)
        {
            S._source.PlayOneShot(S.goalSound, S.sfxVolume);
        }
    }

    public static void PLAY_WIN()
    {
        if (S != null && S.winSound != null)
        {
            S._source.PlayOneShot(S.winSound, S.sfxVolume);
        }
    }

    public static void PLAY_LOSS()
    {
        if (S != null && S.lossSound != null)
        {
            S._source.PlayOneShot(S.lossSound, S.sfxVolume);
        }
    }

    public static void START_WIND()
    {
        if (S != null && S.windSound != null && !S._windSource.isPlaying)
        {
            S._windSource.clip = S.windSound;

            S._windSource.Play();
        }
    }

    public static void STOP_WIND()
    {
        if (S != null && S._windSource.isPlaying)
        {
            S._windSource.Stop();
        }
    }
}