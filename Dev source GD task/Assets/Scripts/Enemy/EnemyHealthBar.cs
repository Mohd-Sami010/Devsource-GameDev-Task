using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar :MonoBehaviour {

    [SerializeField] private Enemy enemy;
    [SerializeField] private Image healthBarImage;

    private void Start()
    {
        enemy.OnEnemyDamage += Enemy_OnEnemyDamage;
        gameObject.SetActive(false);
    }

    private void Enemy_OnEnemyDamage()
    {
        gameObject.SetActive(true);
        healthBarImage.fillAmount = enemy.GetHealthNormalized();
    }
    private void OnDestroy()
    {
        enemy.OnEnemyDamage -= Enemy_OnEnemyDamage;
    }
}
