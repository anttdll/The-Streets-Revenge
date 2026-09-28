using UnityEngine;

public class MenuPause : MonoBehaviour
{
    public GameObject painelPause;
    private bool jogoPausado = false;

    void Start()
    {
        painelPause.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jogoPausado)
            {
                Retomar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Retomar()
    {
        painelPause.SetActive(false);
        Time.timeScale = 1f;
        jogoPausado = false;
    }

    void Pausar()
    {
        painelPause.SetActive(true);
        Time.timeScale = 0f;
        jogoPausado = true;
    }
}
