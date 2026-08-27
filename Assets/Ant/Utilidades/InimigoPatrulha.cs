using UnityEngine;

public class InimigoPerseguidor : MonoBehaviour
{
    [Header("Configurações de Patrulha (Andar pela sala)")]
    public float patrolSpeed = 2f;
    public float patrolChangeDirectionTime = 3f; // Tempo para mudar de direção aleatória

    [Header("Configurações de Perseguição")]
    public float chaseSpeed = 5f;
    public float detectionRange = 5f; // Distância que ele enxerga o player
    public LayerMask playerLayer; // Selecione a camada "Player" aqui no Inspector

    [Header("Referências")]
    public Transform player; // Arraste o Transform do player aqui

    private Rigidbody2D rb;
    private bool isChasing = false;
    private float patrolTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolTimer = patrolChangeDirectionTime;

        // Se não arrastou o player, tenta achar pela tag
        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        // 1. Verifica se o player está no alcance (usando Raycast 2D)
        DetectPlayer();
    }

    void FixedUpdate()
    {
        if (isChasing)
        {
            // 2. Se está perseguindo, vai reto na direção do player
            ChasePlayer();
        }
        else
        {
            // 3. Se não está perseguindo, patrulha a sala
            PatrolRoom();
        }
    }

    // --- Lógica de Detecção ---
    void DetectPlayer()
    {
        if (player == null) return;

        // Cria um Raycast da posição do inimigo até o player
        Vector2 directionToPlayer = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, detectionRange, playerLayer);

        // Se o Raycast acertou o player, começa a perseguir
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
    }

    // --- Lógica de Perseguição (Linha Reta) ---
    void ChasePlayer()
    {
        if (player == null) return;

        // Calcula a direção exata para o player
        Vector2 direction = ((Vector2)player.position - rb.position).normalized;

        // Move o inimigo reto para lá
        rb.linearVelocity = direction * chaseSpeed;

        // (Opcional) Faz o sprite olhar para a direção que está andando
        if (direction.x > 0.1f) transform.localScale = new Vector3(1, 1, 1); // Olhando para direita
        else if (direction.x < -0.1f) transform.localScale = new Vector3(-1, 1, 1); // Olhando para esquerda
    }

    // --- Lógica de Patrulha ---
    void PatrolRoom()
    {
        // Timer para mudar de direção aleatória
        patrolTimer -= Time.deltaTime;

        if (patrolTimer <= 0)
        {
            patrolTimer = patrolChangeDirectionTime;

            // Escolhe uma direção aleatória (horizontal ou vertical)
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);

            // Se estiver muito perto de zero, força para um lado (evita ficar parado)
            if (Mathf.Abs(randomX) < 0.1f) randomX = 1f;
            if (Mathf.Abs(randomY) < 0.1f) randomY = 0f;

            rb.linearVelocity = new Vector2(randomX, randomY).normalized * patrolSpeed;
        }
    }

    // --- Lógica de Reset ao Bater na Parede ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se estava perseguindo e bateu em algo que não é o player
        if (isChasing && !collision.gameObject.CompareTag("Player"))
        {
            // Para a perseguição
            isChasing = false;

            // Para o movimento imediatamente (dá um reset)
            rb.linearVelocity = Vector2.zero;

            // Reseta o timer de patrulha para ele já sair andando em outra direção
            patrolTimer = 0.1f;
        }
    }

    // Para desenhar a linha de detecção no editor (bônus visual)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}