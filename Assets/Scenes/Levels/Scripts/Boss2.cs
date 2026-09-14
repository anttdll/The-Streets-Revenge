using UnityEngine;

public class Boss2 : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 40;
    private int currentHealth;

    [Header("Movimento")]
    public float moveSpeed = 1.5f;
    public Transform player;

    [Header("Inimigos")]
    public GameObject inimigoPrefab;
    public GameObject inimigoShooterPrefab;

    [Header("Spawn")]
    public float spawnInterval = 2f;
    public int enemiesPerWave = 2;
    public int maxEnemiesAlive = 8;
    public float spawnRadius = 2f;

    private float spawnTimer;

    [Header("Animação")]
    public Sprite[] walkFrames;
    public Sprite[] attackFrames;

    public float animationSpeed = 0.15f;
    public float attackAnimDuration = 0.6f;

    private int currentFrame;
    private float animTimer;

    private bool isAttacking = false;
    private float attackTimer;

    [Header("Sala")]
    [HideInInspector] public Room parentRoom;

    [Header("Feedback de dano")]
    public Color hitFlashColor = Color.red;
    public float hitFlashDuration = 0.1f;

    [Header("Áudio")]
    public AudioClip deathSound;

    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private bool isDead = false;

    public int contactDamage = 1;

    private Vector2 randomDirection;
    private float randomMoveTimer;

    public float randomMoveChangeTime = 1.5f;

    public float enemyKnockback = 4f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Inimigo"))
            return;

        Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (enemyRb != null)
        {
            Vector2 direction =
                (collision.transform.position - transform.position).normalized;

            enemyRb.AddForce(direction * enemyKnockback, ForceMode2D.Impulse);

            if (collision.contactCount == 0)
                return;

            Vector2 normal = collision.GetContact(0).normal;

            // Reflete a direção atual na superfície
            randomDirection = Vector2.Reflect(randomDirection, normal).normalized;

            randomMoveTimer = randomMoveChangeTime;
        }
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        spawnTimer = spawnInterval;

        if (parentRoom == null)
            parentRoom = GetComponentInParent<Room>();
    }
    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                player = p.transform;
        }
    }

    bool IsBossRoomActive()
    {
        if (parentRoom == null)
            return false;

        if (CameraController.Instance == null)
            return false;

        if (CameraController.Instance.CurrentRoom != parentRoom)
            return false;

        if (!parentRoom.IsActive)
            return false;

        return true;
    }

    void FixedUpdate()
    {
        if (isDead || player == null)
            return;

        // Mesma lógica do Boss 1
        if (parentRoom != null &&
            (CameraController.Instance.CurrentRoom != parentRoom || !parentRoom.IsActive))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Durante ataque
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Troca a direção aleatoriamente
        randomMoveTimer -= Time.fixedDeltaTime;

        if (randomMoveTimer <= 0f)
        {
            randomDirection = Random.insideUnitCircle.normalized;
            randomMoveTimer = randomMoveChangeTime;
        }

        rb.linearVelocity = randomDirection * moveSpeed;

        if (randomDirection.x != 0)
            sr.flipX = randomDirection.x < 0;
    }
    void Update()
    {
        if (isDead)
            return;

        // FORA DA SALA DO BOSS
        if (!IsBossRoomActive())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // ANIMAÇÃO DE ATAQUE
        if (isAttacking)
        {
            UpdateAttackAnimation();

            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                isAttacking = false;

                currentFrame = 0;
                animTimer = 0f;

                // Volta para o primeiro frame da animação de andar
                if (walkFrames != null && walkFrames.Length > 0)
                    sr.sprite = walkFrames[0];
            }

            return;
        }

        // TIMER DO SPAWN
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            StartAttack();

            spawnTimer = spawnInterval;
        }

        // ANIMAÇÃO DE ANDAR
        UpdateWalkAnimation();
    }

    void StartAttack()
    {
        isAttacking = true;

        attackTimer = attackAnimDuration;

        currentFrame = 0;
        animTimer = 0f;

        // Primeiro frame da animação de ataque
        if (attackFrames != null && attackFrames.Length > 0)
            sr.sprite = attackFrames[0];

        // IMPORTANTE:
        // Os inimigos aparecem quando o ataque começa.
        SpawnWave();
    }

    void UpdateAttackAnimation()
    {
        if (attackFrames == null || attackFrames.Length == 0)
            return;

        animTimer += Time.deltaTime;

        if (animTimer >= animationSpeed)
        {
            animTimer = 0f;

            currentFrame++;

            if (currentFrame >= attackFrames.Length)
                currentFrame = attackFrames.Length - 1;

            sr.sprite = attackFrames[currentFrame];
        }
    }
   
    void UpdateWalkAnimation()
    {
        if (walkFrames == null || walkFrames.Length == 0)
            return;

        animTimer += Time.deltaTime;

        if (animTimer >= animationSpeed)
        {
            animTimer = 0f;

            currentFrame++;

            if (currentFrame >= walkFrames.Length)
                currentFrame = 0;

            sr.sprite = walkFrames[currentFrame];
        }
    }

    void SpawnWave()
    {
        int enemiesAlive = CountEnemies();

        if (enemiesAlive >= maxEnemiesAlive)
            return;

        int amountToSpawn = enemiesPerWave;

        int availableSpace =
            maxEnemiesAlive - enemiesAlive;

        if (amountToSpawn > availableSpace)
            amountToSpawn = availableSpace;

        for (int i = 0; i < amountToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        GameObject prefab = null;

        // 30% de chance de Shooter
        if (inimigoShooterPrefab != null &&
            Random.value < 0.3f)
        {
            prefab = inimigoShooterPrefab;
        }
        else
        {
            prefab = inimigoPrefab;
        }

        if (prefab == null)
            return;

        Vector2 spawnPosition =
            (Vector2)transform.position +
            Random.insideUnitCircle * spawnRadius;

        Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );
    }

    int CountEnemies()
    {
        GameObject[] inimigos =
            GameObject.FindGameObjectsWithTag("Inimigo");

        return inimigos.Length;
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
            return;

        currentHealth -= Mathf.RoundToInt(amount);

        Debug.Log("Boss 2 tomou dano! Vida: " + currentHealth);

        StopCoroutine(nameof(HitFlash));
        StartCoroutine(HitFlash());

        if (currentHealth <= 0)
            Die();
    }

    System.Collections.IEnumerator HitFlash()
    {
        sr.color = hitFlashColor;

        yield return new WaitForSeconds(hitFlashDuration);

        sr.color = Color.white;
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        rb.linearVelocity = Vector2.zero;

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(
                deathSound,
                transform.position
            );
        }

        if (parentRoom != null)
        {
            parentRoom.NotifyEnemyDefeated(gameObject);
        }

        PlayerHealth ph =
            FindFirstObjectByType<PlayerHealth>();

        if (ph != null && GameData.Instance != null)
        {
            GameData.Instance.SaveHealth(
                ph.CurrentHealth,
                ph.MaxHealth
            );
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
            PlayerHealth ph =
                collision.gameObject.GetComponent<PlayerHealth>();

            if (ph != null)
            {
                ph.TakeDamage(contactDamage);
            }
        }
    }
}