using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Referências")]
    public PlayerHealth playerHealth;
    public GameObject heartPrefab; // um prefab de Image (UI) já configurado
    public Transform heartsContainer; // o objeto pai onde os corações vão nascer

    [Header("Sprites")]
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [Header("Layout")]
    public float spacing = 40f;

    private Image[] heartImages;

    void Start()
    {
        int totalHearts = Mathf.CeilToInt(playerHealth.MaxHealth / 2f);
        heartImages = new Image[totalHearts];

        for (int i = 0; i < totalHearts; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsContainer);
            RectTransform rt = heart.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(i * spacing, 0);
            heartImages[i] = heart.GetComponent<Image>();
        }

        UpdateHearts();
    }

    void Update()
    {
        UpdateHearts();
    }

    void UpdateHearts()
    {
        int currentHealth = playerHealth.CurrentHealth;

        for (int i = 0; i < heartImages.Length; i++)
        {
            int heartValue = currentHealth - (i * 2);

            if (heartValue >= 2)
                heartImages[i].sprite = fullHeart;
            else if (heartValue == 1)
                heartImages[i].sprite = halfHeart;
            else
                heartImages[i].sprite = emptyHeart;
        }
    }
}