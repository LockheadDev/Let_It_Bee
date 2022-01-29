using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [Header("GUI Game response")]
    [SerializeField]
    private IGameStateResponse gameStateResponse;
    private void Start()
    {
        try
        {
            gameStateResponse = GetComponent<IGameStateResponse>();
        }
        catch
        {
            Debug.LogError("No Game State Response found!");
        }
        
    }
    void Update()
    {
        Modes gameSatus = GameManager.instance.gameStatus;

        switch (gameSatus)
        {
            case Modes.running:
                gameStateResponse.Resume();
                break;
            case Modes.paused:
                gameStateResponse.Pause();
                break;
            case Modes.over:
                gameStateResponse.Over();
                break;
        }
    }
}
