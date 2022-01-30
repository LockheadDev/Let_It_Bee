using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager instance;

    [Header("Game Status")]
    public Modes gameStatus;

    public int score, lives;

    public int highScore;

    public int hasPlayed;

    public int beesDunked;

    private void Awake()
    {
        gameStatus = Modes.running;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        hasPlayed = PlayerPrefs.GetInt("HasPlayed");
        if (hasPlayed == 0) FirstTime();
        DontDestroyOnLoad(gameObject);
    }

    void GetHighScore()
    {
        if (PlayerPrefs.HasKey("highScore"))
            highScore = PlayerPrefs.GetInt("highScore");
    }

    void SaveHighScores()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
        }
    }

    public void IncrementScore(int num)
    {
        score += num;
    }

    public void IncrementBees(int num)
    {
        beesDunked += num;
    }

    public void DecrementScore(int num)
    {
        score -= num;
    }
    public void IncrementLives(int num)
    {
        lives += num;
    }
    public void DecrementLives(int num)
    {
        lives -= num;
        if(lives<=0)
        {
            gameStatus = Modes.over;
            EndGame();
        }
    }


    public void StartGame()
    {

        gameStatus = Modes.running;
        GetHighScore();

    }
    public void PauseGame()
    {
        gameStatus = Modes.paused;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        gameStatus = Modes.running;
        Time.timeScale = 1;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GetHighScore();

        lives = 3;
        score = 0;
        beesDunked = 0;
        Time.timeScale = 1;
        gameStatus = Modes.running;
    }

    public void EndGame()
    {
        AudioManager.instance.PlayAudio(AudioClp.gameOver);
        SaveHighScores();
        Time.timeScale = 0;
        gameStatus = Modes.over;
    }

    private void FirstTime()
    {
        Debug.Log("First Time");
        gameStatus = Modes.paused;
        Time.timeScale = 0;
        PlayerPrefs.SetInt("HasPlayed", 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}