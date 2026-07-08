using UnityEngine;
using UnityEngine.SceneManagement;

public class ColetarItens : MonoBehaviour
{
    public int itensColetados = 0;
    public int totalItensNecessarios = 5;
    public string proximaCena = "batalha2";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gato"))
        {
            Destroy(other.gameObject);
            itensColetados++;

            if (itensColetados >= totalItensNecessarios)
            {
                TrocarDeCena();
            }
        }
    }

    void TrocarDeCena()
    {
        SceneManager.LoadScene(proximaCena);
    }
}
