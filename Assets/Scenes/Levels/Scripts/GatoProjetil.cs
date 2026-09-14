using UnityEngine;

public class GatoProjetil : MonoBehaviour
{
    [Header("Configurações")]
    public float speed = 12f;
    public float damage = 1f;
    public float lifeTime = 2f;

    [Header("Animação")]
    public Sprite[] frames;
    public float animationSpeed = 0.15f;

    private SpriteRenderer sr;
    private int currentFrame = 0;
    private float timer;

    private Vector2 direction;
    private Rigidbody2D rb;
    private bool hasHit = false;

    [Header("Efeitos")]
    public GameObject hitEffect;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);

        if (frames != null && frames.Length > 0)
        {
            sr.sprite = frames[0];
        }
    }

    void Update()
    {
        AnimateProjectile();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    void AnimateProjectile()
    {
        if (frames == null || frames.Length <= 1)
            return;

        timer += Time.deltaTime;

        if (timer >= animationSpeed)
        {
            timer = 0f;
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                currentFrame = 0;
            }

            sr.sprite = frames[currentFrame];
        }
    }

    public void Launch(Vector2 dir)
    {
        direction = dir.normalized;

        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        // =========================
        // BOSS 1
        // =========================

        Boss boss = other.GetComponent<Boss>();

        if (boss != null)
        {
            hasHit = true;

            boss.TakeDamage(damage);

            SpawnHitEffect();

            Destroy(gameObject);
            return;
        }

        // =========================
        // BOSS 2
        // =========================

        Boss2 boss2 = other.GetComponent<Boss2>();

        if (boss2 != null)
        {
            hasHit = true;

            boss2.TakeDamage(damage);

            SpawnHitEffect();

            Destroy(gameObject);
            return;
        }

        // =========================
        // INIMIGOS
        // =========================

        if (other.CompareTag("Inimigo"))
        {
            hasHit = true;

            Inimigo inimigo =
                other.GetComponent<Inimigo>();

            if (inimigo != null)
            {
                inimigo.TakeDamage(damage);
            }

            InimigoAtirador atirador =
                other.GetComponent<InimigoAtirador>();

            if (atirador != null)
            {
                atirador.TakeDamage(damage);
            }

            SpawnHitEffect();

            Destroy(gameObject);
            return;
        }

        // =========================
        // PAREDE
        // =========================

        if (other.CompareTag("Wall"))
        {
            hasHit = true;

            Destroy(gameObject);
        }
    }

    void SpawnHitEffect()
    {
        if (hitEffect != null)
        {
            Instantiate(
                hitEffect,
                transform.position,
                Quaternion.identity
            );
        }
    }
}