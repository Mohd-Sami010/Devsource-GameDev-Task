using UnityEngine;

public class PauseUI :MonoBehaviour {
    private void Start()
    {
        GameManager.Instance.OnGamePaused += () => gameObject.SetActive(true);
        GameManager.Instance.OnGameResumed += () => gameObject.SetActive(false);

        gameObject.SetActive(false);
    }
}
