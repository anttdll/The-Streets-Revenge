using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 6;
    public int currentHealth;

    [Header("Invulnerabilidade")]
    public float invincibilityTime = 0.5f;
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;

    [Header("UI")]
    public GameObject heartPrefab;
    public Transform heartContainer;

    [Header("Efeitos")]
    public GameObject hitEffect;
    public AudioClip hitSound;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateHealthUI();
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = true;
                    spriteRenderer.color = Color.white;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        Debug.Log($"Player tomou dano! Vida: {currentHealth}/{maxHealth}");

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        isInvincible = true;
        invincibilityTimer = invincibilityTime;

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (heartContainer == null || heartPrefab == null) return;

        // Limpa os corações antigos
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }

        // Cria corações cheios (vida atual)
        for (int i = 0; i < currentHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            Image image = heart.GetComponent<Image>();
            if (image != null)
            {
                image.color = Color.red; // Coração cheio
            }
        }

        // Cria corações vazios (vida perdida)
        for (int i = currentHealth; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            Image image = heart.GetComponent<Image>();
            if (image != null)
            {
                image.color = Color.gray; // Coração vazio
            }
        }
    }

    void Die()
    {
        Debug.Log("Game over");
       
    }
}