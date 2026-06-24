using UnityEngine;

public class DestruicaoCasa : MonoBehaviour
{
    [Header("Sprites de Estado")]
    public Sprite spriteMeioDestruido;
    public Sprite spriteCompletamenteDestruido;

    [Header("Configuracao de Batidas")]
    public int batidasParaMeioDestruir = 3;
    public int batidasParaDestruirTotal = 5;

    private SpriteRenderer spriteRenderer;
    private int contadorColisoes = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        contadorColisoes++;

        if (contadorColisoes == batidasParaMeioDestruir)
        {
            if (spriteMeioDestruido != null)
            {
                spriteRenderer.sprite = spriteMeioDestruido;
            }
        }
        else if (contadorColisoes >= batidasParaDestruirTotal)
        {
            if (spriteCompletamenteDestruido != null)
            {
                spriteRenderer.sprite = spriteCompletamenteDestruido;
            }
        }
    }
}
