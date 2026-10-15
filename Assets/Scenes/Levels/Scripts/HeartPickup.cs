using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public int healAmount = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph == null || ph.IsFullHealth) return; // com vida cheia, o coração fica no chão

        ph.Heal(healAmount);
        Destroy(gameObject);
    }
}