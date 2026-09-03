using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 30;
    private int currentHealth;

    [Header("Movimento")]
    public float moveSpeed = 2f;
    public Transform player;

    [Header("Ataque - Rajada radial")]
    public GameObject radialProjetilPrefab;
    public int radialShotCount = 10;
    public float radialSpeedRandomness = 0.2f;

    [Header("Ataque - Tiros que caem")]
    public GameObject lobbedProjetilPrefab;
    public int lobbedShotCount = 4;
    public float lobbedSpreadRadius = 4f;

    [Header("Timing")]
    public float attackInterval = 3f;
    private float attackTimer;

    [Header("Sala")]
    [HideInInspector] public Room parentRoom;

    [Header("Feedback de dano")]
    public Color hitFlashColor = Color.red;
    public float hitFlashDuration = 0.1f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool isDead = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        attackTimer = attackInterval;

        if (parentRoom == null) parentRoom = GetComponentInParent<Room>();
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

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
    }

    void Update()
    {
        if (isDead || player == null) return;
        if (parentRoom != null && (CameraController.Instance.CurrentRoom != parentRoom || !parentRoom.IsActive)) return;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            AttackBurst();
            attackTimer = attackInterval;
        }
    }

    void AttackBurst()
    {
        // 1. Rajada radial (tiros retos espalhados em círculo)
        if (radialProjetilPrefab != null)
        {
            float angleStep = 360f / radialShotCount;
            float startAngle = Random.Range(0f, 360f); // varia o padrão a cada rajada

            for (int i = 0; i < radialShotCount; i++)
            {
                float angle = startAngle + (angleStep * i);
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                GameObject proj = Instantiate(radialProjetilPrefab, transform.position, Quaternion.identity);
                BossProjetil bp = proj.GetComponent<BossProjetil>();
                if (bp != null) bp.Launch(dir);
            }
        }

        // 2. Tiros que sobem e caem, espalhados perto do player
        if (lobbedProjetilPrefab != null)
        {
            for (int i = 0; i < lobbedShotCount; i++)
            {
                Vector2 randomOffset = Random.insideUnitCircle * lobbedSpreadRadius;
                Vector3 targetPos = (Vector3)( (Vector2)player.position + randomOffset );

                GameObject proj = Instantiate(lobbedProjetilPrefab, transform.position, Quaternion.identity);
                BossLobbedProjetil lp = proj.GetComponent<BossLobbedProjetil>();
                if (lp != null) lp.Launch(transform.position, targetPos);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= (int)amount;
        StopCoroutine(nameof(HitFlash));
        StartCoroutine(HitFlash());

        if (currentHealth <= 0) Die();
    }

    System.Collections.IEnumerator HitFlash()
    {
        sr.color = hitFlashColor;
        yield return new WaitForSeconds(hitFlashDuration);
        sr.color = Color.white;
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        if (parentRoom != null) parentRoom.NotifyEnemyDefeated(gameObject);
        Destroy(gameObject);
    }
}