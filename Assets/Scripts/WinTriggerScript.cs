using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTriggerScript : MonoBehaviour
{
    public LogicManager logic;
    public UiManager uiManager;
    LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManager>();
        uiManager = GameObject.FindGameObjectWithTag("Logic").GetComponent<UiManager>();
        levelManager = GameObject.FindGameObjectWithTag("Logic").GetComponent<LevelManager>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!uiManager.isMenuOpen)
        {
            Taptic.Success();

            if(levelManager.currentLevelIndex < 99) {
                logic.GameWin();
            } else
            {
                logic.HandleFinish();
            }
        }
    }
}
