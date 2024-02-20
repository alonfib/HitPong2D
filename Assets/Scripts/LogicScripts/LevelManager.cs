using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject gameLevels; // Reference to the GameLevels GameObject
    public int currentLevelIndex = 0; // Current level index
    public GameObject confetti;

    private LogicManager logic;
    private UiManager uiManager;

    public int maxLevel;

    // Start is called before the first frame update
    void Start()
    {
        logic = gameObject.GetComponent<LogicManager>();
        uiManager = gameObject.GetComponent<UiManager>();
        InitializeLevels();
    }

    public void PrevLevel()
    {
        if (currentLevelIndex > 0)
        {
            confetti.SetActive(false);
            MoveToLevel(currentLevelIndex - 1);
            logic.RestartGame();
        }
        else
        {
            Debug.Log("Already at the first level!");
        }
    }

    public void NextLevel()
    {
        if (currentLevelIndex < logic.userProgression)
        {
            confetti.SetActive(false);
            MoveToLevel(currentLevelIndex + 1);
            logic.RestartGame();
        }
        else
        {
            Debug.Log("No more levels to unlock!");
        }
    }

    public void MoveToLevel(int levelIndex)
    {
        confetti.SetActive(false);
        GetLevel(currentLevelIndex).SetActive(false);
        GetLevel(levelIndex).SetActive(true);
        currentLevelIndex = levelIndex;
        uiManager.UpdateLevelButtons();
        uiManager.HandleGameLevelText(levelIndex);
    }

    public void ResetLevel()
    {
        GetLevel(currentLevelIndex).SetActive(false);
        GetLevel(currentLevelIndex).SetActive(true);
    }

    private void InitializeLevels()
    {
        // Deactivate all levels except the first one
        maxLevel = gameLevels.transform.childCount;

        for (int i = 0; i < gameLevels.transform.childCount; i++)
        {
            GetLevel(i).SetActive(i == 0);
        }
        uiManager.UpdateLevelButtons();
    }

    private GameObject GetLevel(int index)
    {
        return gameLevels.transform.GetChild(index).gameObject;
    }
}
