using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [Header("Game values")]
    [SerializeField]
    private TextMeshProUGUI score;
    private ILiveResponse liveResponse;
    private IGameStateResponse gameStateResponse;

    private void Awake()
    {
        liveResponse = GetComponent<ILiveResponse>();
        gameStateResponse = GetComponent<IGameStateResponse>();
    }
    void Update()
    {
        score.text = GameManager.instance.score.ToString();
        liveResponse.SetLive(GameManager.instance.lives);

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
                break;
            default:
                break;
        }
    }
}

