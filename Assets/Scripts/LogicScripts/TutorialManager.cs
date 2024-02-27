using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour
{
    public GameObject TutorialObject; // Reference to the GameLevels GameObject
    public GameObject TutorialPages; // Reference to the GameLevels GameObject
    public GameObject GameUI;
    public GameObject Menu;


    public bool IsTutorialDone = true;

    const string tutorialKey = "TutorialKey";
    const string tutorialDoneString = "Done";
    UiManager UiManager;

    int currentPage = 0;
    // Use this for initialization
    void Start()
    {
        UiManager = gameObject.GetComponent<UiManager>();
        IsTutorialDone = PlayerPrefs.GetString(tutorialKey, "") == tutorialDoneString; // Default to 0 if not set
        if (IsTutorialDone)
        {
            TutorialObject.SetActive(false);
        } else
        {
            StartTutorial();
        }

    }

    public void StartTutorial()
    {
        if (Menu.activeInHierarchy)
        {
            UiManager.ToggleMenu(false);
        }

        GameUI.SetActive(false);
        TutorialObject.SetActive(true);
        GetTutorialPage(currentPage).SetActive(true);
    }

    public void NextTutorialPage()
    {
        if(currentPage + 1 < 5)
        {
            GetTutorialPage(currentPage).SetActive(false);
            GetTutorialPage(currentPage + 1).SetActive(true);
            currentPage = currentPage + 1;
        } else
        {
            GetTutorialPage(currentPage).SetActive(false);
            HandleEndTutorial();
        }   
    }

    private GameObject GetTutorialPage(int index)
    {
        return TutorialPages.transform.GetChild(index).gameObject;
    }

    void HandleEndTutorial()
    {
        GameUI.SetActive(true);
        TutorialObject.SetActive(false);
        PlayerPrefs.SetString(tutorialKey, tutorialDoneString);
        currentPage = 0;
  
    }
}
