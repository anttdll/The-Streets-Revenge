using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    [Header("Vida salva")]
    public int savedCurrentHealth = -1;
    public int savedMaxHealth = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveHealth(int current, int max)
    {
        savedCurrentHealth = current;
        savedMaxHealth = max;
    }

    public bool HasSavedHealth()
    {
        return savedCurrentHealth >= 0;
    }

    public void ResetGame()
    {
        savedCurrentHealth = -1;
        savedMaxHealth = -1;
    }
}