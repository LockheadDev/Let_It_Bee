using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager instance;

    [Header("Game Status")]
    public Modes gameStatus = Modes.running;

    public int score, lives;

    public int highScore;


    

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    void GetHighScore()
    {
        
        if (PlayerPrefs.HasKey("highScore"))
            highScore = PlayerPrefs.GetInt("highScore");
    }

    void SaveHighScores()
    {
        if (score > highScore)
            PlayerPrefs.SetInt("highScore", score);
    }

    public void IncrementScore(int num)
    {
        score += num;
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
        Time.timeScale = 1;
        gameStatus = Modes.running;
    }
    public void EndGame()
    {

        Time.timeScale = 0;
        gameStatus = Modes.over;
        print("Game Over!");
        SaveHighScores();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}