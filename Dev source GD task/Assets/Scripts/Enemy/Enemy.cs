using UnityEngine;
using System.Collections;

public class Enemy :MonoBehaviour {
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("References")]
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private SpriteRenderer bodySpriteRenderer;

    public event System.Action OnEnemyDamage;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (PlayerController.Instance == null) return;

        Vector2 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
        transform.position += moveSpeed * Time.deltaTime * (Vector3)direction;

        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRed());
        OnEnemyDamage?.Invoke();
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        GameObject deathEffectObj = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathEffectObj, 3f);

        Destroy(gameObject);
    }

    private IEnumerator FlashRed()
    {
        Color bodyColor = bodySpriteRenderer.color;
        if (bodySpriteRenderer)
        {
            bodySpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            bodySpriteRenderer.color = bodyColor;
        }
    }
    public float GetHealthNormalized()
    {
        return (float)currentHealth / maxHealth;
    }
}
