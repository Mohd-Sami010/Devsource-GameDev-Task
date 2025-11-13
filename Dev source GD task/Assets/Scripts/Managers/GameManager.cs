using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager :MonoBehaviour {
    public static GameManager Instance { private set; get; }

    private int coinsCollected = 0;
    public event System.Action<int> OnCoinCollected;
    public enum GameState {
        Playing,
        Paused,
        GameOver
    }
    private GameState currentState = GameState.Playing;

    public event System.Action OnGamePaused;
    public event System.Action OnGameResumed;

    public enum GameOverType {
        Victory,
        Died
    }
    public event EventHandler<OnGameOverEventArgs> OnGameOver;
    public class OnGameOverEventArgs :EventArgs {
        public GameOverType gameOverType;
        public int coinsCollected;
    }
    public event System.Action OnLoading;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void TogglePauseGame()
    {
        if (currentState != GameState.Paused)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0f;
            OnGamePaused?.Invoke();
            //Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
            OnGameResumed?.Invoke();
            //Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public GameState GetCurrentGameState()
    {
        return currentState;
    }
    public void GameOver(GameOverType gameOverType)
    {
        currentState = GameState.GameOver;
        OnGameOver?.Invoke(this, new OnGameOverEventArgs
        {
            gameOverType = gameOverType,
            coinsCollected = coinsCollected
        });
        Time.timeScale = 0f;

        if (gameOverType == GameOverType.Victory)
        {
            int maxLevelIndex = 3;
            int nextlevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (nextlevelIndex <= maxLevelIndex) PlayerPrefs.SetInt("LevelToContinue", nextlevelIndex);
            else PlayerPrefs.SetInt("LevelToContinue", 1);
        }

        //Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        OnLoading?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        OnLoading?.Invoke();
        int maxLevelIndex = 3;
        int nextlevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextlevelIndex <= maxLevelIndex) SceneManager.LoadScene(nextlevelIndex);
        else SceneManager.LoadScene(0);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        OnLoading?.Invoke();
        SceneManager.LoadScene(0);
    }
    public void CollectCoin(int coinValue)
    {
        coinsCollected += coinValue;
        string highScoreName = SceneManager.GetActiveScene().name + "_" + "HighScore";
        if (PlayerPrefs.GetInt(highScoreName, 0) < coinsCollected)
        {
            PlayerPrefs.SetInt(highScoreName, coinsCollected);
        }
        OnCoinCollected?.Invoke(coinsCollected);
    }
    public int GetCoinsCollected()
    {
        return coinsCollected;
    }
}
