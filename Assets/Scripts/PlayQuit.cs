using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitPlay : MonoBehaviour
{


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

   public void Jogar()
    {
        SceneManager.LoadScene("AdotarGatos");
    }


    public void Quitar()
    {
        Application.Quit();
        Debug.Log("saiu");
    }

}
