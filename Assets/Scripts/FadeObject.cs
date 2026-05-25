using UnityEngine;

public class FadeObject : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color c = sr.color;
            c.a = 0.4f; // transparência
            sr.color = c;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color c = sr.color;
            c.a = 1f; // normal
            sr.color = c;
        }
    }
}