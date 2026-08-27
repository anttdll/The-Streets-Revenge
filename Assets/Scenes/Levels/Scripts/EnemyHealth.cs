using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    [HideInInspector] public Room parentRoom; // atribuído automaticamente ou manualmente

    private void Awake()
    {
        currentHealth = maxHealth;

        // se não atribuído manualmente, tenta achar a sala automaticamente
        if (parentRoom == null)
        {
            parentRoom = GetComponentInParent<Room>();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (parentRoom != null)
        {
            parentRoom.NotifyEnemyDefeated(gameObject);
        }
        Destroy(gameObject);
    }
}