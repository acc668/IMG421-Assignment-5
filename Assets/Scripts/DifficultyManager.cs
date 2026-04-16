using UnityEngine;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance 
    { 
        get; 
        private set; 
    }

    [Header("Dynamic")]
    public Difficulty current = Difficulty.Easy;

    [Header("Music")]
    public AudioClip musicClip;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;

    private AudioSource _musicSource;

    public static int ShotLimit()
    {
        switch (Instance != null ? Instance.current : Difficulty.Easy)
        {
            case Difficulty.Easy:
            {
                return -1;
            }

            case Difficulty.Medium:
            {
                return 5;
            }

            case Difficulty.Hard:
            {
                return 3;
            }

            default:
            {
                return -1;
            }
        }
    }

    public static Vector2 WindRange()
    {
        switch (Instance != null ? Instance.current : Difficulty.Easy)
        {
            case Difficulty.Easy:
            {
                return Vector2.zero;
            }

            case Difficulty.Medium:
            {
                return new Vector2(2f, 6f);
            }

            case Difficulty.Hard:
            {
                return new Vector2(6f, 12f);
            }

            default:
            {
                return Vector2.zero;
            }
        }
    }

    public static bool GoalMoves()
    {
        return Instance != null && Instance.current == Difficulty.Hard;
    }

    public static bool AimAssistEnabled()
    {
        return Instance != null && Instance.current == Difficulty.Easy;
    }

    public static bool SlowMoEnabled()
    {
        return Instance != null && Instance.current == Difficulty.Easy;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            _musicSource = gameObject.AddComponent<AudioSource>();

            _musicSource.clip = musicClip;

            _musicSource.loop = true;

            _musicSource.volume = musicVolume;

            _musicSource.playOnAwake = false;

            if (musicClip != null)
            {
                _musicSource.Play();
            }
        }

        else
        {
            Destroy(gameObject);
        }
    }
}