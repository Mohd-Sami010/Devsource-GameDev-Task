using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI :MonoBehaviour {

    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI playButtonTextMesh;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button playReProgramButton;
    [SerializeField] private GameObject loadingScreen;

    private void Awake()
    {
        int levelToPlay = 1;
        if (PlayerPrefs.GetInt("LevelToContinue", 0) > 1)
        {
            playButtonTextMesh.text = "Continue";
            levelToPlay = PlayerPrefs.GetInt("LevelToContinue", 0);
        }

        playButton.onClick.AddListener(() => {
            loadingScreen.SetActive(true);
            SceneManager.LoadScene(levelToPlay);
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
        playReProgramButton.onClick.AddListener(() => {
            Application.OpenURL("https://samicode-games.itch.io/re-program");
        });

    }
}
