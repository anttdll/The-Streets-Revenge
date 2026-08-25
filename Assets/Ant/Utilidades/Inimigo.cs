using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [Header("Status")]
    public float maxHealth = 3f;
    public float moveSpeed = 2f;
    public float attackRange = 1.5f;
    public int damage = 1;
    public float attackCooldown = 1f;

    [Header("IA")]
    public float visionRange = 5f;
    public float stopDistance = 1f; // Distância que para de perseguir

    [Header("Drop")]
    public GameObject dropItem;
    public float dropChance = 0.3f;

    private float currentHealth;
    private float attackTimer = 0f;
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // IA de perseguição
        if (distanceToPlayer < visionRange && distanceToPlayer > stopDistance)
        {
            // Persegue o jogador
            Vector2 direction = ((Vector2)player.position - rb.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            // Vira o sprite
            if (direction.x != 0)
                spriteRenderer.flipX = direction.x < 0;
        }
        else if (distanceToPlayer <= stopDistance)
        {
            // Parado, pronto para atacar
            rb.linearVelocity = Vector2.zero;

            // Ataca se estiver perto
            if (!isAttacking)
            {
                Attack();
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Cooldown de ataque
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            isAttacking = false;
        }
    }

    void Attack()
    {
        if (attackTimer > 0) return;

        isAttacking = true;
        attackTimer = attackCooldown;

        // Animação de ataque
        if (animator != null)
            animator.SetTrigger("Attack");

        // Causa dano no player
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("Inimigo atacou!");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Efeito de dano (pisca vermelho)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            Invoke(nameof(ResetColor), 0.1f);
        }

        Debug.Log($"Inimigo tomou dano! Vida: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }

    void Die()
    {
        Debug.Log("Inimigo morreu!");

        // Drop de item
        if (dropItem != null && Random.value < dropChance)
        {
            Instantiate(dropItem, transform.position, Quaternion.identity);
        }

        // Efeito de morte (partículas, som)
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Se colidir com o player, ataca
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}