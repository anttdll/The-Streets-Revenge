using UnityEngine;

public class Alimentar : MonoBehaviour
{
    public SpriteRenderer gato;          // SpriteRenderer do gato
    public Sprite gatoAlimentando;       // Novo sprite

    public void Alimentacao()
    {
        gato.sprite = gatoAlimentando;
    }
}