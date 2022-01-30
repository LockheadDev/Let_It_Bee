using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIPackage : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    public void ResumeButton()
    {
        GameManager.instance.ResumeGame();
        pauseMenu.SetActive(false);
    }

}
