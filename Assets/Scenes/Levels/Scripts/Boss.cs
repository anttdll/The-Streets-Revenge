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

    [Header("Ataque - Tiros que caem")]
    public GameObject lobbedProjetilPrefab;
    public int lobbedShotCount = 4;
    public float lobbedSpreadRadius = 4f;

    [Header("Timing")]
    public float attackInterval = 3f;
    private float attackTimer;
    public float attackAnimDuration = 0.5f;

    [Header("Animação")]
    public Sprite[] walkFrames;
    public Sprite[] attackFrames;
    public float animationSpeed = 0.15f;
    private int currentFrame = 0;
    private float animTimer = 0f;
    private bool isAttackingAnim = false;
    private float attackAnimTimer = 0f;

    [Header("Sala")]
    [HideInInspector] public Room parentRoom;

    [Header("Feedback de dano")]
    public Color hitFlashColor = Color.red;
    public float hitFlashDuration = 0.1f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool isDead = false;
    public int contactDamage = 1;

    [Header("Áudio")]
    public AudioClip deathSound;

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

        if (isAttackingAnim)
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
        if (isDead || player == null) return;
        if (parentRoom != null && (CameraController.Instance.CurrentRoom != parentRoom || !parentRoom.IsActive))
        {
            AnimateAndamento();
            return;
        }

        if (isAttackingAnim)
        {
            attackAnimTimer -= Time.deltaTime;
            if (attackAnimTimer <= 0f) isAttackingAnim = false;
        }

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            StartAttackAnim();
            AttackBurst();
            attackTimer = attackInterval;
        }

        AnimateAndamento();
    }

    void StartAttackAnim()
    {
        isAttackingAnim = true;
        attackAnimTimer = attackAnimDuration;
        currentFrame = 0;
        animTimer = 0f;
    }

    void AnimateAndamento()
    {
        Sprite[] currentSet = isAttackingAnim ? attackFrames : walkFrames;
        if (currentSet == null || currentSet.Length == 0) return;

        animTimer += Time.deltaTime;
        if (animTimer >= animationSpeed)
        {
            animTimer = 0f;
            currentFrame = (currentFrame + 1) % currentSet.Length;
            sr.sprite = currentSet[currentFrame];
        }
    }

    void AttackBurst()
    {
        if (radialProjetilPrefab != null)
        {
            float angleStep = 360f / radialShotCount;
            float startAngle = Random.Range(0f, 360f);

            for (int i = 0; i < radialShotCount; i++)
            {
                float angle = startAngle + (angleStep * i);
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                GameObject proj = Instantiate(radialProjetilPrefab, transform.position, Quaternion.identity);
                BossProjetil bp = proj.GetComponent<BossProjetil>();
                if (bp != null) bp.Launch(dir);
            }
        }

        if (lobbedProjetilPrefab != null)
        {
            for (int i = 0; i < lobbedShotCount; i++)
            {
                Vector2 randomOffset = Random.insideUnitCircle * lobbedSpreadRadius;
                Vector3 targetPos = (Vector3)((Vector2)player.position + randomOffset);

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

        if (deathSound != null) AudioSource.PlayClipAtPoint(deathSound, transform.position);
        if (parentRoom != null) parentRoom.NotifyEnemyDefeated(gameObject);

        // salva a vida do player antes de trocar de cena
        PlayerHealth ph = FindFirstObjectByType<PlayerHealth>();
        if (ph != null && GameData.Instance != null)
        {
            GameData.Instance.SaveHealth(ph.CurrentHealth, ph.MaxHealth);
        }

        if (VictoryManager.Instance != null)
        {
            VictoryManager.Instance.ShowVictory();
        }

        Destroy(gameObject);
    }

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