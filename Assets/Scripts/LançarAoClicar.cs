using UnityEngine;

public class LancarAoClicar : MonoBehaviour
{
    [Header("Configurações de Lançamento")]
    public GameObject projetilPrefab; 
    public Transform pontoDeLancamento; // De onde o gato sai
    public float forcaDoTiro = 10f; // Força que sai o tiro

    private void OnMouseDown()
    {
        // verifica se o prefab existe para evitar erros
        if (projetilPrefab != null && pontoDeLancamento != null)
        {
            // cria uma cópia do projétil na posição e rotação do ponto de lançamento
            GameObject novoProjetil = Instantiate(projetilPrefab, pontoDeLancamento.position, pontoDeLancamento.rotation);

            //  pega o rigidbody do gato e faz uma força para frente
            Rigidbody rb = novoProjetil.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(pontoDeLancamento.forward * forcaDoTiro, ForceMode.Impulse);
            }

           
        }
    }
}
