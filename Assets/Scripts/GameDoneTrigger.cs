using UnityEngine;
using System.Collections;

public class GameDoneTrigger : MonoBehaviour
{
    UiManager uiManager;
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("Logic").GetComponent<UiManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        {
            Taptic.Success();
            uiManager.HandleFinishGameScreen();
        }
    }
}
