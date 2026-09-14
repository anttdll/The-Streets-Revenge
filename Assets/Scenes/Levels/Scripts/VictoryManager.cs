using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance;

    [Header("UI")]
    public GameObject victoryPanel;

    [Header("Próxima cena")]
    public string nextSceneName;
    public string menu;
    public string Recomeca;

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

    public void GoToNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    public void Menuzin()
    {
        Time.timeScale = 1f;

        if (GameData.Instance != null)
        {
            GameData.Instance.ResetGame();
        }

        SceneManager.LoadScene(menu);
    }


    public void Restart()
    {
        Time.timeScale = 1f;

        // Reseta os dados do jogo
        if (GameData.Instance != null)
        {
            GameData.Instance.ResetGame();
        }


        SceneManager.LoadScene(Recomeca);

    }
}
