using UnityEngine;

public class BossProjetil : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;
    public float lifeTime = 4f;

    private Rigidbody2D rb;
    private Vector2 direction;
    private bool hasHit = false;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Start() => Destroy(gameObject, lifeTime);

    public void Launch(Vector2 dir)
    {
        direction = dir.normalized;
        rb.linearVelocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Player"))
        {
            hasHit = true;
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(damage);
            Destroy(gameObject);
        }
        if (other.CompareTag("Wall")) Destroy(gameObject);
    }
}