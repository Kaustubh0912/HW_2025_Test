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
            if (ScoreManager.Instance.GetScore() > 40)
            {
                EndGame("Victory");
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
        GameOverMessage = message;
        SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        
        enabled = false;
    }
}
