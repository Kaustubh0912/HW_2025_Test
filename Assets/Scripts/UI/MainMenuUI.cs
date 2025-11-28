using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Toggle limitedModeToggle;
    [SerializeField] private Toggle endlessModeToggle;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            if (limitedModeToggle != null && limitedModeToggle.isOn)
            {
                GameSettings.CurrentGameMode = GameMode.Limited;
            }
            else if (endlessModeToggle != null && endlessModeToggle.isOn)
            {
                GameSettings.CurrentGameMode = GameMode.Endless;
            }
            
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Start()
    {
        playButton.Select();
    }
}
