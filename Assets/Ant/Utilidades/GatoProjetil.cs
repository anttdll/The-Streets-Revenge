using UnityEngine;

public class GatoProjetil : MonoBehaviour
{
    [Header("Configurações")]
    public float speed = 12f;
    public float damage = 1f;
    public float lifeTime = 2f;

    private Vector2 direction;
    private Rigidbody2D rb;
    private bool hasHit = false;

    [Header("Efeitos")]
    public GameObject hitEffect;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    public void Launch(Vector2 dir)
    {
        direction = dir.normalized;

        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Inimigo"))
        {
            hasHit = true;

            Inimigo inimigo = other.GetComponent<Inimigo>();

            if (inimigo != null)
            {
                inimigo.TakeDamage(damage);
            }

            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}