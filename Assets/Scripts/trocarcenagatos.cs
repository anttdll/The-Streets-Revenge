using UnityEngine;
using UnityEngine.SceneManagement;

public class trocarcenagatos : MonoBehaviour
{
    public int totalItensNecessarios = 5;
    public string proximaCena = "Batalha2";

    void Update()
    {
        if (destruit.pegarItem >= totalItensNecessarios)
        {
            SceneManager.LoadScene(proximaCena);
        }
    }
}