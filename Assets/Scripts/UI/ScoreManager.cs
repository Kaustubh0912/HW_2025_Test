using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score;

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
        ResetScore();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    public void IncrementScore()
    {
        score++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public int GetScore()
    {
        return score;
    }
}
