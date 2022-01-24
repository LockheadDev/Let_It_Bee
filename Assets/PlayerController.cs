using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetKeyboardInput();
    }

    private void GetKeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.gameStatus == Modes.running)
        {
            GameManager.instance.PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.gameStatus == Modes.paused)
        {
            GameManager.instance.ResumeGame();
        }
        if((GameManager.instance.gameStatus == Modes.paused || GameManager.instance.gameStatus == Modes.over) && Input.GetKeyDown(KeyCode.R))
        {
            GameManager.instance.RestartGame();
        }

    }
}
