using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 8f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private string currentAnimation = "";

    // Controle de ataque
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float attackDuration = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        // ===== MOVIMENTO =====
        movement.x = 0;
        movement.y = 0;

        if (Input.GetKey(KeyCode.W)) movement.y = 1;
        if (Input.GetKey(KeyCode.S)) movement.y = -1;
        if (Input.GetKey(KeyCode.A)) movement.x = -1;
        if (Input.GetKey(KeyCode.D)) movement.x = 1;

        movement.Normalize();

        // ===== TIMER DE ATAQUE =====
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
            }
        }

        // ===== ATUALIZA ANIMAÇÃO =====
        UpdateAnimations();
        UpdateSpriteFlip();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    void UpdateAnimations()
    {
        if (animator == null) return;

        bool isMoving = movement != Vector2.zero;

        // ===== SE ESTIVER ATACANDO =====
        if (isAttacking)
        {
            // ESCOLHE A ANIMAÇÃO DE ATAQUE CERTA
            string attackAnimation = isMoving ? "AttackWalk" : "AttackIdle";

            if (currentAnimation != attackAnimation)
            {
                animator.Play(attackAnimation);
                currentAnimation = attackAnimation;
            }
            return;
        }

        // ===== SE NÃO ESTIVER ATACANDO =====
        string animationName = isMoving ? "Walk" : "Idle";

        if (currentAnimation != animationName)
        {
            animator.Play(animationName);
            currentAnimation = animationName;
        }
    }

    void UpdateSpriteFlip()
    {
        if (spriteRenderer == null) return;

        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
    }

    public void PlayAttackAnimation()
    {
        isAttacking = true;
        attackTimer = attackDuration;
    }
}