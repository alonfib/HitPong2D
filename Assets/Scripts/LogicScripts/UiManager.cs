using UnityEngine.UI;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject menuScreen;
    public GameObject gameUI;

    public Button levelUpButton;
    public Button levelDownButton;
    public Text levelText; // Public reference to the Text component
    public Text menuLevelText; // Public reference to the Text component
    public BallScript ballScript;
    public GameObject gameFinishedScreen;

    public bool isMenuOpen = false;

    private LogicManager logic;
    private LevelManager levelManager;



    void Start()
    {
        logic = gameObject.GetComponent<LogicManager>();
        levelManager = gameObject.GetComponent<LevelManager>();   
    }

    public void ToggleMenu(bool isOpen)
    {
        ballScript.FreezeBall(isOpen);
        isMenuOpen = isOpen;
        menuScreen.SetActive(isOpen);
        gameUI.SetActive(!isOpen);
        logic.RestartGame();
    }

    public void UpdateLevelButtons()
    {
        if (levelUpButton != null)
        {
            levelUpButton.interactable = levelManager.currentLevelIndex < logic.userProgression && levelManager.currentLevelIndex < 100;
        }
        else
        {
            Debug.Log("levelUpButton == null");
        }

        if (levelDownButton != null)
        {
            levelDownButton.interactable = levelManager.currentLevelIndex > 0;
        }
        else
        {
            Debug.Log("levelDownButton == null");
        }
    }

    public void HandleGameLevelText(int levelIndex)
    {
        if (levelText == null)
        {
            Debug.LogError("Level Text component not assigned!");
            return;
        }

        int formattedLevelText = levelIndex + 1;
        levelText.text = "Level " + formattedLevelText;
        menuLevelText.text = "Level " + formattedLevelText;
    }

    public void HandleFinishGameScreen()
    {
        gameFinishedScreen.SetActive(true);
    }

    public void ToggleFinishScreen()
    {
        gameFinishedScreen.SetActive(false);
        logic.RestartGame();
    }
}
