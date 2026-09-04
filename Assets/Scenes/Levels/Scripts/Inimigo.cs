using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Inimigo : MonoBehaviour
{
    [Header("Vida")]
    public float maxHealth = 5f;
    private float currentHealth;

    [Header("Movimento")]
    public float moveSpeed = 2.5f;
    public Transform player;

    [Header("Feedback de dano")]
    public Color hitFlashColor = Color.red;
    public float hitFlashDuration = 0.1f;

    [Header("Áudio")]
    public AudioClip deathSound;

    [Header("Linha de visão")]
    public LayerMask obstacleLayer;

    [Header("Animação")]
    public Sprite[] walkFrames;
    public float animationSpeed = 0.15f;
    private int currentFrame = 0;
    private float animTimer = 0f;

    [Header("Sala")]
    [HideInInspector] public Room parentRoom;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool isDead = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (parentRoom == null)
            parentRoom = GetComponentInParent<Room>();
    }

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void FixedUpdate()
    {
        if (isDead || player == null) return;

        if (parentRoom != null && (CameraController.Instance.CurrentRoom != parentRoom || !parentRoom.IsActive))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (!HasLineOfSightToPlayer())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;

        if (dir.x != 0) sr.flipX = dir.x < 0;
    }

    void Update()
    {
        AnimateWalk();
    }

    void AnimateWalk()
    {
        if (walkFrames.Length == 0) return;

        // só anima se estiver realmente se movendo
        bool isMoving = rb.linearVelocity.sqrMagnitude > 0.01f;
        if (!isMoving) return;

        animTimer += Time.deltaTime;
        if (animTimer >= animationSpeed)
        {
            animTimer = 0f;
            currentFrame = (currentFrame + 1) % walkFrames.Length;
            sr.sprite = walkFrames[currentFrame];
        }
    }

    private bool HasLineOfSightToPlayer()
    {
        Vector2 origin = transform.position;
        Vector2 target = player.position;
        RaycastHit2D hit = Physics2D.Linecast(origin, target, obstacleLayer);
        return hit.collider == null;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        StopAllCoroutines();
        StartCoroutine(HitFlash());

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator HitFlash()
    {
        sr.color = hitFlashColor;
        yield return new WaitForSeconds(hitFlashDuration);
        sr.color = Color.white;
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        if (deathSound != null) AudioSource.PlayClipAtPoint(deathSound, transform.position);

        if (parentRoom != null)
        {
            parentRoom.NotifyEnemyDefeated(gameObject);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        if (player == null) return;
        Gizmos.color = HasLineOfSightToPlayer() ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, player.position);
    }

    [Header("Dano ao encostar")]
    public int contactDamage = 1;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(contactDamage);
            }
        }
    }
}