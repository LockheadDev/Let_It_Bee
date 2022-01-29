using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Space]

    [Header("Key Actions")]
    [SerializeField]
    private KeyCode pauseButton;
    [SerializeField]
    private KeyCode restartButton;

    void Update()
    {
        GetKeyboardInput();
    }

    private void GetKeyboardInput()
    {
        if(Input.GetKeyDown(pauseButton) && GameManager.instance.gameStatus == Modes.running)
        {
            GameManager.instance.PauseGame();
        }
        else if (Input.GetKeyDown(pauseButton) && GameManager.instance.gameStatus == Modes.paused)
        {
            GameManager.instance.ResumeGame();
        }
        if((GameManager.instance.gameStatus == Modes.paused || GameManager.instance.gameStatus == Modes.over) && Input.GetKeyDown(restartButton))
        {
            GameManager.instance.RestartGame();
            GameManager.instance.StartGame();
        }

    }
}
