using UnityEngine;

public class destruit : MonoBehaviour
{
    public static int pegarItem = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pegarItem++;
            Destroy(gameObject);
        }
    }
}