using UnityEngine;

public class canhaoRotate : MonoBehaviour
{
    public float velocidade = 90f;
    public float anguloMin = -28f;
    public float anguloMax = 40f;

    private float anguloAtual = 0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            anguloAtual += velocidade * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            anguloAtual -= velocidade * Time.deltaTime;
        }

        // Limita entre -40 e 40
        anguloAtual = Mathf.Clamp(anguloAtual, anguloMin, anguloMax);

        // Aplica a rotação
        transform.rotation = Quaternion.Euler(0, 0, anguloAtual);
    }
}