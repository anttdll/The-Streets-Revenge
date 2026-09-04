using UnityEngine;

public class InimigoAtirador : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 4;
    private int currentHealth;

    [Header("Movimento")]
    public float moveSpeed = 1.5f;
    public float preferredDistance = 5f;
    public Transform player;

    [Header("Tiro")]
    public GameObject projetilPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    private float fireTimer = 0f;

    [Header("Feedback de dano")]
    public Color hitFlashColor = Color.red;
    public float hitFlashDuration = 0.1f;

    [Header("Áudio")]
    public AudioClip deathSound;

    [Header("Linha de visão")]
    public LayerMask obstacleLayer;

    [Header("Animação")]
    public Sprite[] walkFrames;
    public Sprite[] attackFrames;
    public float animationSpeed = 0.15f;
    public float attackAnimDuration = 0.3f;
    private int currentFrame = 0;
    private float animTimer = 0f;
    private bool isAttackingAnim = false;
    private float attackAnimTimer = 0f;

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

        if (parentRoom == null) parentRoom = GetComponentInParent<Room>();

        if (firePoint == null)
        {
            GameObject fp = new GameObject("FirePoint");
            fp.transform.parent = transform;
            fp.transform.localPosition = Vector3.zero;
            firePoint = fp.transform;
        }
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

        float dist = Vector2.Distance(transform.position, player.position);
        Vector2 dirToPlayer = (player.position - transform.position).normalized;

        if (dist < preferredDistance - 0.5f)
        {
            rb.linearVelocity = -dirToPlayer * moveSpeed;
        }
        else if (dist > preferredDistance + 0.5f)
        {
            rb.linearVelocity = dirToPlayer * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (dirToPlayer.x != 0) sr.flipX = dirToPlayer.x < 0;
    }

    void Update()
    {
        if (isDead || player == null) return;

        bool canAct = parentRoom == null || (CameraController.Instance.CurrentRoom == parentRoom && parentRoom.IsActive);

        if (canAct && HasLineOfSightToPlayer())
        {
            if (isAttackingAnim)
            {
                attackAnimTimer -= Time.deltaTime;
                if (attackAnimTimer <= 0f) isAttackingAnim = false;
            }

            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                Shoot();
                StartAttackAnim();
                fireTimer = fireRate;
            }
        }

        AnimateSprite();
    }

    void StartAttackAnim()
    {
        isAttackingAnim = true;
        attackAnimTimer = attackAnimDuration;
        currentFrame = 0;
        animTimer = 0f;
    }

    void AnimateSprite()
    {
        Sprite[] currentSet = isAttackingAnim ? attackFrames : walkFrames;
        if (currentSet == null || currentSet.Length == 0) return;

        bool isMoving = rb.linearVelocity.sqrMagnitude > 0.01f;
        if (!isMoving && !isAttackingAnim) return;

        animTimer += Time.deltaTime;
        if (animTimer >= animationSpeed)
        {
            animTimer = 0f;
            currentFrame = (currentFrame + 1) % currentSet.Length;
            sr.sprite = currentSet[currentFrame];
        }
    }

    private bool HasLineOfSightToPlayer()
    {
        Vector2 origin = transform.position;
        Vector2 target = player.position;
        RaycastHit2D hit = Physics2D.Linecast(origin, target, obstacleLayer);
        return hit.collider == null;
    }

    void Shoot()
    {
        if (projetilPrefab == null) return;

        Vector2 dir = (player.position - firePoint.position).normalized;
        GameObject proj = Instantiate(projetilPrefab, firePoint.position, Quaternion.identity);
        InimigoProjetil ip = proj.GetComponent<InimigoProjetil>();
        if (ip != null) ip.Launch(dir);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= (int)amount;
        StopAllCoroutines();
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
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        if (player == null) return;
        Gizmos.color = HasLineOfSightToPlayer() ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, player.position);
    }
}