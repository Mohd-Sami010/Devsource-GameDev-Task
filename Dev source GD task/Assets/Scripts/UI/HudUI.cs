using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudUI :MonoBehaviour {

    [SerializeField] private TextMeshProUGUI coinsTextMesh;
    [SerializeField] private TextMeshProUGUI ammoTextMesh;
    [SerializeField] private RectTransform crosshairRectTransform;
    [SerializeField] private Image healthBarImage;

    private void Start()
    {
        GameManager.Instance.OnCoinCollected += GameManager_OnCoinCollected;
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;

        PlayerController.Instance.OnPlayerDamage += PlayerController_OnPlayerDamage;
    }

    private void PlayerController_OnPlayerDamage()
    {
        healthBarImage.fillAmount = PlayerController.Instance.GetHealthNormalized();
    }

    private void Update()
    {
        crosshairRectTransform.position = InputManager.Instance.GetAimInput();
        ammoTextMesh.text = PlayerShoot.Instance.GetAmmoText();
    }
    private void GameManager_OnGamePaused()
    {
        gameObject.SetActive(false);
    }
    private void GameManager_OnGameResumed()
    {
        gameObject.SetActive(true);
    }
    private void GameManager_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void GameManager_OnCoinCollected(int coinsCollected)
    {
        coinsTextMesh.text = coinsCollected.ToString();
    }
}
