using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 6; // ex: 6 = 3 corações inteiros, se cada coração vale 2
    private int currentHealth;

    [Header("Invencibilidade após dano")]
    public float invincibilityDuration = 1f;
    private bool isInvincible = false;

    [Header("Feedback visual")]
    public float flashInterval = 0.1f;

    private SpriteRenderer sr;
    private bool isDead = false;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible || isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFlash());
        }
    }

    private System.Collections.IEnumerator InvincibilityFlash()
    {
        isInvincible = true;
        float timer = 0f;

        while (timer < invincibilityDuration)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        sr.enabled = true;
        isInvincible = false;
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player morreu!");

        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.ShowGameOver();
        }

        // desativa movimento/ataque, mas não destrói o objeto ainda
        // (evita erros de referência null em outros scripts)
        GetComponent<PlayerAttack>().enabled = false;
        gameObject.SetActive(false);
    }
}