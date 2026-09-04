using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance;

    [Header("UI")]
    public GameObject victoryPanel;

    private void Awake()
    {
        Instance = this;
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    public void ShowVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MenuCarregar()
    {
        SceneManager.LoadScene("Menu");
    }
}