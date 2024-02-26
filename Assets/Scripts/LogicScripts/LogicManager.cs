using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;
    public GameObject FinishScreen;
    public BallScript ballScript;
    public int userProgression = 0;
    public GameObject confetti;
    public bool ResetProgression = false;

    private bool isBallTouchedPaddle = false;
    private UiManager uiManager;
    private LevelManager levelManager;
    private UnityAds adsManager;

    private const string UserProgressionKey = "UserProgression";


    private void Start()
    {

        if (ResetProgression)
        {
            PlayerPrefs.SetInt(UserProgressionKey, 0);
        }

        adsManager = gameObject.GetComponent<UnityAds>();
        uiManager = gameObject.GetComponent<UiManager>();
        levelManager = gameObject.GetComponent<LevelManager>();
        userProgression = PlayerPrefs.GetInt(UserProgressionKey, 0); // Default to 0 if not set
        levelManager.MoveToLevel(userProgression);
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    [ContextMenu("Restart Game")]
    public void RestartGame()
    {
        if(isBallTouchedPaddle)
        {
            adsManager.HandleInGameAds(adsManager.looseAdPoints);
        }

        levelManager.ResetLevel();
        isBallTouchedPaddle = false;
        confetti.SetActive(false);
        gameOverScreen.SetActive(false);
        gameWinScreen.SetActive(false);
        ballScript.resetBall();
    }

    public void GameWin()
    {
        if (isBallTouchedPaddle)
        {
            confetti.SetActive(true);
            gameWinScreen.SetActive(true);
            if (levelManager.currentLevelIndex == userProgression)
            {
                IncreaseUserProgression();
            }
            adsManager.HandleInGameAds(adsManager.winAdPoints);
            uiManager.UpdateLevelButtons();
        }
        else
        {
            GameOver();
        }
    }

    public void AdIncreaseProgression()
    {
        if(userProgression < levelManager.maxLevel)
        {
            userProgression = userProgression + 1;
            uiManager.UpdateLevelButtons();
        }
    }

    public void TouchedPaddle()
    {
        isBallTouchedPaddle = true;
    }

    private void IncreaseUserProgression()
    {
        userProgression++;
        PlayerPrefs.SetInt(UserProgressionKey, userProgression); // Save progression
        PlayerPrefs.Save(); // Ensure it's written to disk
    }
}
