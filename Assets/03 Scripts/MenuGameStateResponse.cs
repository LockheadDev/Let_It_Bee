using UnityEngine;

public class MenuGameStateResponse : MonoBehaviour, IGameStateResponse
{
    [Header("Menu Settings")]
    [SerializeField]
    private GameObject pauseGUI;
    [SerializeField]
    private GameObject gameOverGUI;

    private void Update()
    {
        gameObject.SetActive(true);
    }
    private void Awake()
    {
        pauseGUI.SetActive(false);
        gameOverGUI.SetActive(false);
    }
    public void Pause()
    {
        pauseGUI.SetActive(true);
    }

    public void Resume()
    {
        pauseGUI.SetActive(false);
        gameOverGUI.SetActive(false);
    }

    public void Over()
    {
        gameOverGUI.SetActive(true);
    }
}
