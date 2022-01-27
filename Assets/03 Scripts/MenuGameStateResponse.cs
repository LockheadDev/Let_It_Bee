using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameStateResponse : MonoBehaviour, IGameStateResponse
{
    [Header("Menu Settings")]
    [SerializeField]
    private GameObject pauseMenu;
    private void Start()
    {
        pauseMenu.SetActive(false);
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }


}
