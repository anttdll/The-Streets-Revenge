using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDeCenas : MonoBehaviour
{
    public void MudarCena(string AdotarGatos)
    {
        SceneManager.LoadScene(AdotarGatos);
    }
}
