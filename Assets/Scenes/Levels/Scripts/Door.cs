using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Sprites (opcional)")]
    public Sprite openSprite;
    public Sprite closedSprite;

    private Collider2D physicalCollider;
    private SpriteRenderer sr;
    private bool isLocked = false;

    private void Awake()
    {
        physicalCollider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        Unlock(); // porta começa aberta por padrão
    }

    public void Lock()
    {
        isLocked = true;
        physicalCollider.enabled = true; // bloqueia passagem
        if (sr != null && closedSprite != null) sr.sprite = closedSprite;
    }

    public void Unlock()
    {
        isLocked = false;
        physicalCollider.enabled = false; // libera passagem
        if (sr != null && openSprite != null) sr.sprite = openSprite;
    }

    public bool IsLocked => isLocked;
}