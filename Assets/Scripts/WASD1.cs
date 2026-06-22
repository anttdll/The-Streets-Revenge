using UnityEngine;

public class MovimentoJogador : MonoBehaviour
{
    public float velocidadeInput = 5f;
    private Animator animator;
    private Rigidbody2D rb; 
    private Vector2 movimento;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        movimento.x = Input.GetAxisRaw("Horizontal");
        movimento.y = Input.GetAxisRaw("Vertical");

        
        if (movimento != Vector2.zero)
        {
            animator.SetFloat("MoveX", movimento.x);
            animator.SetFloat("MoveY", movimento.y);
        }

        
        animator.SetFloat("Velocidade", movimento.sqrMagnitude);
    }

    void FixedUpdate()
    {
       
        rb.MovePosition(rb.position + movimento.normalized * velocidadeInput * Time.fixedDeltaTime);
    }
}
