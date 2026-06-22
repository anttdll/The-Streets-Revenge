using UnityEngine;

public class wasd : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movimento;

    void Start()
    {
        // Captura os componentes automaticamente do mesmo Objeto
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Captura as teclas WASD ou Setas do teclado (-1, 0 ou 1)
        movimento.x = Input.GetAxisRaw("Horizontal");
        movimento.y = Input.GetAxisRaw("Vertical");

        // Atualiza a direção no Animator apenas se o jogador estiver se movendo.
        // Isso faz o personagem continuar olhando para a última direção quando parar.
        if (movimento != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movimento.x);
            animator.SetFloat("Vertical", movimento.y);
        }

        // Informa ao Animator se o jogador está parado (0) ou se movendo (maior que 0)
        animator.SetFloat("Speed", movimento.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // Aplica a física de movimento de forma suave e normalizada para evitar andar mais rápido na diagonal
        rb.MovePosition(rb.position + movimento.normalized * velocidade * Time.fixedDeltaTime);
    }
}