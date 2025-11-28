using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() => 
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
        playButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Start()
    {
        if (messageText != null)
        {
            messageText.text = GameManager.GameOverMessage;
        }
    }
}
