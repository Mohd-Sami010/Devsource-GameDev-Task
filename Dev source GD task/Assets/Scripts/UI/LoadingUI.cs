using UnityEngine;

public class LoadingUI :MonoBehaviour {

    private void Start()
    {
        GameManager.Instance.OnLoading += GameManager_OnLoading;
        gameObject.SetActive(false);
    }

    private void GameManager_OnLoading()
    {
        gameObject.SetActive(true);
    }
}
