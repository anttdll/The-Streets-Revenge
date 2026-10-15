using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public int healAmount = 2;
    public float pickupDelay = 0.6f; // tempo antes de poder pegar

    private float spawnTime;

    private void Awake()
    {
        spawnTime = Time.time;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time - spawnTime < pickupDelay) return;
        if (!other.CompareTag("Player")) return;

        PlayerHealth ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null || ph.IsFullHealth) return;

        ph.Heal(healAmount);
        Destroy(gameObject);
    }
}