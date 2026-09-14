using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitPlay : MonoBehaviour
{

    
     [Header("Painéis")]
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;

    public void OpenCredits()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

   public void Jogar()
    {
        SceneManager.LoadScene("1");

        if (GameData.Instance != null)
        {
            GameData.Instance.ResetGame();
        }
    }


    public void Quitar()
    {
        Application.Quit();
        Debug.Log("saiu");
    }



}
