using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode 
{ 
    idle, 
    playing, 
    levelEnd 
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Inscribed")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitShotsRemaining;
    public Text uitDifficulty;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("UI – Result / Menu")]
    public MenuManager menuManager;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public int shotsRemaining;
    public GameObject castle;
    public GameMode mode = GameMode.idle;

    void Start()
    {
        S = this;

        level = 0;

        shotsTaken = 0;

        shotsRemaining = DifficultyManager.ShotLimit();

        levelMax = castles.Length;

        UpdateDifficultyLabel();

        StartLevel();
    }

    void StartLevel()
    {
        if (castle != null) 
        {
            Destroy(castle);
        }

        Projectile.DESTROY_PROJECTILES();

        castle = Instantiate<GameObject>(castles[level]);

        castle.transform.position = castlePos;

        Goal.goalMet = false;

        shotsRemaining = DifficultyManager.ShotLimit();

        UpdateGUI();

        mode = GameMode.playing;

        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI()
    {
        if (uitLevel != null)
        {
            int displayLevel = Mathf.Min(level + 1, levelMax);

            uitLevel.text = "Level: " + displayLevel + " of " + levelMax;
        }

        if (uitShots != null)
        {
            uitShots.text = "Shots Fired: " + shotsTaken;
        }

        if (uitShotsRemaining != null)
        {
            if (shotsRemaining < 0)
            {
                uitShotsRemaining.gameObject.SetActive(false);
            }

            else
            {
                uitShotsRemaining.gameObject.SetActive(true);

                uitShotsRemaining.text = "Shots Left: " + shotsRemaining;

                uitShotsRemaining.color = shotsRemaining <= 1 ? Color.red : Color.white;
            }
        }
    }

    void UpdateDifficultyLabel()
    {
        if (uitDifficulty == null || DifficultyManager.Instance == null) 
        {
            return;
        }

        switch (DifficultyManager.Instance.current)
        {
            case Difficulty.Easy:
            {
                uitDifficulty.text = "SCOUT MISSION";
                
                break;
            }

            case Difficulty.Medium: 
            {
                uitDifficulty.text = "ASSAULT RUN";
                
                break;
            }

            case Difficulty.Hard:
            {
                uitDifficulty.text = "SIEGE PROTOCOL";
                
                break;
            }
        }
    }

    void Update()
    {
        if (mode != GameMode.idle)
        {
            UpdateGUI();
        }

        if (mode != GameMode.playing) 
        {
            return;
        }

        Projectile[] projs = FindObjectsOfType<Projectile>();

        foreach (Projectile p in projs)
        {
            if (p.transform.position.y < -10f)
            {
                Destroy(p.gameObject);
            }
        }

        if (Goal.goalMet)
        {
            mode = GameMode.levelEnd;

            FollowCam.SWITCH_VIEW(FollowCam.eView.both);

            Invoke("NextLevel", 2f);

            return;
        }

        if (shotsRemaining == 0 && !ProjectileInFlight())
        {
            Debug.Log("Loss condition met! menuManager: " + menuManager);
            
            mode = GameMode.levelEnd;

            Invoke("TriggerLoss", 1.5f);
        }
    }

    void NextLevel()
    {
        level++;

        if (level >= levelMax)
        {
            level = levelMax;

            mode = GameMode.idle;

            UpdateGUI();

            if (menuManager != null) 
            {
                menuManager.ShowWin(shotsTaken);
            }

            return;
        }

        StartLevel();
    }

    void TriggerLoss()
    {
        if (menuManager != null) 
        {
            menuManager.ShowLoss();
        }
    }

    static public void SHOT_FIRED()
    {
        S.shotsTaken++;

        if (S.shotsRemaining > 0) 
        {
            S.shotsRemaining--;
        }
    }

    static public bool OUT_OF_SHOTS()
    {
        return S.shotsRemaining == 0;
    }

    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }

    bool ProjectileInFlight()
    {
        // A projectile is in flight if any Projectile component is awake
        Projectile[] projs = FindObjectsOfType<Projectile>();

        foreach (Projectile p in projs)
        {
            if (p.awake) 
            {
                return true;
            }
        }
        
        return false;
    }
}