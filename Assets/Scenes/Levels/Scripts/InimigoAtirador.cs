using UnityEngine;

public class InimigoAtirador : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 4;
    private int currentHealth;

    [Header("Movimento")]
    public float moveSpeed = 1.5f;
    public float preferredDistance = 5f; // distância que ele tenta manter do player
    public Transform player;

    [Header("Tiro")]
    public GameObject projetilPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    private float fireTimer = 0f;

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
        if (!HasLineOfSightToPlayer())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isDead || player == null) return;

        // só age se o player estiver na mesma sala
        if (parentRoom != null && (CameraController.Instance.CurrentRoom != parentRoom || !parentRoom.IsActive))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);
        Vector2 dirToPlayer = (player.position - transform.position).normalized;

        // mantém distância: se está muito perto, foge um pouco; se muito longe, aproxima
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
    }

    void Update()
    {
        if (isDead || player == null) return;
        if (parentRoom != null && (CameraController.Instance.CurrentRoom != parentRoom || !parentRoom.IsActive)) return;
        if (!HasLineOfSightToPlayer()) return; // não atira se não tiver linha de visão

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
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

        if (parentRoom != null) parentRoom.NotifyEnemyDefeated(gameObject);
        Destroy(gameObject);
    }

    [Header("Linha de visão")]
    public LayerMask obstacleLayer; // define no Inspector: só a layer "Obstaculo"

    private bool HasLineOfSightToPlayer()
    {
        Vector2 origin = transform.position;
        Vector2 target = player.position;

        RaycastHit2D hit = Physics2D.Linecast(origin, target, obstacleLayer);

        // se o Linecast não acertou nada na layer de obstáculo, a visão está livre
        return hit.collider == null;
    }
}