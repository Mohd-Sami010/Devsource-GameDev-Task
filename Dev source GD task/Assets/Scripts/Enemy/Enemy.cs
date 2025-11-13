using UnityEngine;
using System.Collections;

public class Enemy :MonoBehaviour {
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float timeBetweenShots = 1f;
    [SerializeField] private int bulletDamage = 15;

    [Header("References")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private SpriteRenderer bodySpriteRenderer;
    //[SerializeField] private AudioSource audioSource;
    //[SerializeField] private AudioClip deathSound;

    public event System.Action OnEnemyDamage;

    private int currentHealth;
    private bool readyToShoot = true;

    private void Start()
    {
        currentHealth = maxHealth;
        //if (!audioSource)
        //    audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (PlayerController.Instance == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= sightRange)
        {
            ChasePlayer();
        }

        FlipSprite();
    }

    private void ChasePlayer()
    {
        Vector2 dir = (PlayerController.Instance.transform.position - transform.position).normalized;
        transform.position += (Vector3)dir * moveSpeed * Time.deltaTime;
    }

    private void AttackPlayer()
    {
        if (!readyToShoot) return;

        readyToShoot = false;

        Vector2 dir = (PlayerController.Instance.transform.position - shootPoint.position).normalized;
        GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        EnemyBullet enemyBullet = bulletObj.GetComponent<EnemyBullet>();
        if (enemyBullet != null)
            enemyBullet.Shoot(dir, bulletDamage);

        Invoke(nameof(ResetShoot), timeBetweenShots);
    }

    private void ResetShoot()
    {
        readyToShoot = true;
    }

    private void FlipSprite()
    {
        if (PlayerController.Instance == null) return;

        Vector2 direction = PlayerController.Instance.transform.position - transform.position;
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
        if (deathEffect)
        {
            GameObject deathEffectObj = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(deathEffectObj, 3f);
        }

        //if (audioSource && deathSound)
        //    audioSource.PlayOneShot(deathSound);

        Destroy(gameObject);
    }

    private IEnumerator FlashRed()
    {
        if (!bodySpriteRenderer) yield break;

        Color originalColor = bodySpriteRenderer.color;
        bodySpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        bodySpriteRenderer.color = originalColor;
    }

    public float GetHealthNormalized()
    {
        return (float)currentHealth / maxHealth;
    }

    // Optional: visualize ranges in Scene View
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
