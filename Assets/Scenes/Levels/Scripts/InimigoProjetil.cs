using UnityEngine;

public class InimigoProjetil : MonoBehaviour
{
    [Header("Configurações")]
    public float speed = 8f;
    public int damage = 1;
    public float lifeTime = 3f;

    [Header("Animação (opcional)")]
    public Sprite[] frames;
    public float animationSpeed = 0.15f;

    private SpriteRenderer sr;
    private int currentFrame = 0;
    private float timer;
    private Vector2 direction;
    private Rigidbody2D rb;
    private bool hasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
        if (frames.Length > 0) sr.sprite = frames[0];
    }

    void Update()
    {
        AnimateProjectile();
    }

    void AnimateProjectile()
    {
        if (frames.Length <= 1) return;
        timer += Time.deltaTime;
        if (timer >= animationSpeed)
        {
            timer = 0;
            currentFrame = (currentFrame + 1) % frames.Length;
            sr.sprite = frames[currentFrame];
        }
    }

    public void Launch(Vector2 dir)
    {
        direction = dir.normalized;
        if (rb != null) rb.linearVelocity = direction * speed;

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

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}