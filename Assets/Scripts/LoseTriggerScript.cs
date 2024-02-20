using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseTriggerScript : MonoBehaviour
{
    public LogicManager logic;
    public UiManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManager>();
        uiManager = GameObject.FindGameObjectWithTag("Logic").GetComponent<UiManager>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!uiManager.isMenuOpen && !logic.gameWinScreen.activeInHierarchy) 
        {
            logic.GameOver();
        }
    }
}
