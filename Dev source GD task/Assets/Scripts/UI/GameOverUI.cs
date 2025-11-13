using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI :MonoBehaviour {

    [SerializeField] private TextMeshProUGUI gameOverTextMesh;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;
    [SerializeField] private TextMeshProUGUI highScoreTextMesh;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextButton;

    private void Start()
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver; ;

        restartButton.onClick.AddListener(() => {
            GameManager.Instance.RestartLevel();
        });
        nextButton.onClick.AddListener(() => {
            GameManager.Instance.LoadNextLevel();
        });

        gameObject.SetActive(false);
    }

    private void GameManager_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        gameObject.SetActive(true);

        scoreTextMesh.text = "Score: " + e.coinsCollected;
        highScoreTextMesh.text = "High Score: " + PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_" + "HighScore", 0);

        if (e.gameOverType == GameManager.GameOverType.Victory)
        {
            gameOverTextMesh.text = "You Win!";
            nextButton.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(false);
        }
        else
        {
            gameOverTextMesh.text = "Game Over";
            nextButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
        }
    }
}
