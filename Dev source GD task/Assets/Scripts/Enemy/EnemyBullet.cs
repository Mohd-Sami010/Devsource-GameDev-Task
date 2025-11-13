using UnityEngine;

public class EnemyBullet :MonoBehaviour {
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;

    private int damage;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Shoot(Vector2 direction, int damageAmount)
    {
        damage = damageAmount;
        rb.velocity = direction.normalized * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
