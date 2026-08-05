using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDeCenas : MonoBehaviour
{
    public void MudarCena()
    {
        SceneManager.LoadScene("AdotarGatos");
    }
}
