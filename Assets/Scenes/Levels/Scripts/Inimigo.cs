using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Inimigo : MonoBehaviour
{
    [Header("Vida")]
    public float maxHealth = 5f;
    private float currentHealth;

    [Header("Movimento")]
    public float moveSpeed = 2.5f;
    public Transform player; // arrasta o Player aqui, ou deixa vazio pra achar por tag

    [Header("Feedback de dano")]
    public Color hitFlashColor = Color.red;
    public float hitFlashDuration = 0.1f;

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

        // só persegue se o player estiver na mesma sala deste inimigo
        if (parentRoom != null && CameraController.Instance.CurrentRoom != parentRoom)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
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

        if (parentRoom != null)
        {
            parentRoom.NotifyEnemyDefeated(gameObject);
        }

        Destroy(gameObject);
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