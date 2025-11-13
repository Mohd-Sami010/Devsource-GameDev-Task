using UnityEngine;

public class Bullet :MonoBehaviour {

    private Rigidbody2D bulletRigidbody;
    [SerializeField] private float speed = 10f;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        Destroy(gameObject, 3f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(PlayerShoot.Instance.GetBulletDamage());
        }
        Destroy(gameObject);
    }
    public void ShootBulletPrefab(int direction = 1)
    {
        bulletRigidbody.velocity = direction * speed * transform.right;
    }
}
