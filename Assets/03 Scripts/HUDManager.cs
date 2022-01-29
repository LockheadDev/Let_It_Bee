using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("HUD Game values")]
    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private TextMeshProUGUI highScore;

    private ILiveResponse liveResponse;

    private void Start()
    {
        try
        {
            liveResponse = GetComponent<ILiveResponse>();
        }
        catch
        {
            Debug.LogError("No Live Response found!");
        }
    }
    void Update()
    {
        score.text = GameManager.instance.score.ToString();
        highScore.text = GameManager.instance.highScore.ToString();
        liveResponse.SetLive(GameManager.instance.lives);
    }
}

