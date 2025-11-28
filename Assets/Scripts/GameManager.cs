using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerController player;

    public static string GameOverMessage = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerController>();
        }
    }

    private void Update()
    {
        if (ScoreManager.Instance != null)
        {
            // Only check for victory condition if in Limited mode
            if (GameSettings.CurrentGameMode == GameMode.Limited)
            {
                if (ScoreManager.Instance.GetScore() > 40)
                {
                    EndGame("Victory");
                }
            }
        }

        if (player != null)
        {
            if (player.transform.position.y < -2f)
            {
                EndGame("Game Lost");
            }
        }
    }

    private void EndGame(string message)
    {
        if (GameSettings.CurrentGameMode == GameMode.Endless && message == "Game Lost")
        {
            int currentScore = ScoreManager.Instance.GetScore();
            int highScore = PlayerPrefs.GetInt("HighScore", 0);

            if (currentScore > highScore)
            {
                highScore = currentScore;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
                message = $"New High Score!\nScore: {currentScore}";
            }
            else
            {
                message = $"Game Over\nScore: {currentScore}\nHigh Score: {highScore}";
            }
        }

        GameOverMessage = message;
        SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        
        enabled = false;
    }
}
