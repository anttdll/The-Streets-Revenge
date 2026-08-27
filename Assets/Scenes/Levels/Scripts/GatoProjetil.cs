using UnityEngine;

public class GatoProjetil : MonoBehaviour
{
    [Header("Configurações")]
    public float speed = 12f;
    public float damage = 1f;
    public float lifeTime = 2f;

    [Header("Animação")]
    public Sprite[] frames; // Arraste os 3 Sprites fatiados aqui
    public float animationSpeed = 0.15f; // Velocidade da troca de quadros
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

        // Garante que a bala comece no primeiro quadro da animação
        if (frames.Length > 0)
        {
            sr.sprite = frames[0];
        }
    }

    void Update()
    {
        // Lógica da animação (roda sempre)
        AnimateProjectile();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    // Método para fazer o loop da animação
    void AnimateProjectile()
    {
        if (frames.Length <= 1) return; // Se não tiver frames, não anima

        timer += Time.deltaTime;

        if (timer >= animationSpeed)
        {
            timer = 0;
            currentFrame++;

            // Se chegou no último quadro, volta para o primeiro (loop)
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