using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Title Panel")]
    public GameObject titlePanel;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    [Header("Result Panel (Game Scene)")]
    public GameObject resultPanel;
    public Text resultTitleText;
    public Text resultBodyText;
    public Button menuButton;

    void Start()
    {
        if (easyButton != null) 
        {
            easyButton.onClick.AddListener(() => StartGame(Difficulty.Easy));
        }

        if (mediumButton != null) 
        {
            mediumButton.onClick.AddListener(() => StartGame(Difficulty.Medium));
        }

        if (hardButton != null) 
        {
            hardButton.onClick.AddListener(() => StartGame(Difficulty.Hard));
        }

        if (menuButton != null) 
        {
            menuButton.onClick.AddListener(ReturnToMenu);
        }

        if (resultPanel != null) 
        {
            resultPanel.SetActive(false);
        }
    }

    public void ShowWin(int shotsTaken)
    {
        if (resultPanel == null) 
        {
            return;
        }

        resultPanel.SetActive(true);

        SoundManager.PLAY_WIN();

        SoundManager.STOP_WIND();

        if (resultTitleText != null) 
        {
            resultTitleText.text = "MISSION COMPLETE";
        }

        if (resultBodyText != null) 
        {
            resultBodyText.text = "Alien outpost destroyed!\nShots fired: " + shotsTaken + "\n\nReturn to base?";
        }
    }

    public void ShowLoss()
    {
        if (resultPanel == null) 
        {
            return;
        }

        resultPanel.SetActive(true);

        SoundManager.PLAY_LOSS();

        SoundManager.STOP_WIND();

        if (resultTitleText != null) 
        {
            resultTitleText.text = "MISSION FAILED";
        }

        if (resultBodyText != null) 
        {
            resultBodyText.text = "You ran out of ammunition.\nThe alien outpost survives...\n\nReturn to base?";
        }
    }

    public void ShowSatelliteLoss()
    {
        if (resultPanel == null) 
        {
            return;
        }

        resultPanel.SetActive(true);

        SoundManager.PLAY_LOSS();

        SoundManager.STOP_WIND();

        Time.timeScale = 1f;

        if (resultTitleText != null) 
        {
            resultTitleText.text = "SATELLITE HIT!";
        }

        if (resultBodyText != null) 
        {
            resultBodyText.text = "You destroyed a government satellite!\nMission aborted by Space Command.\n\nReturn to base?";
        }
    }

    void StartGame(Difficulty diff)
    {
        DifficultyManager.Instance.current = diff;

        SceneManager.LoadScene(1);
    }

    void ReturnToMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }
}