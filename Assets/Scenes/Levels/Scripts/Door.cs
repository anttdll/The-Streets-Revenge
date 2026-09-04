using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Sprites (opcional)")]
    public Sprite openSprite;
    public Sprite closedSprite;

    private Collider2D physicalCollider;
    private SpriteRenderer sr;
    private bool isLocked = false;

    [Header("Áudio")]
    public AudioClip lockSound;
    public AudioClip unlockSound;
    private void Awake()
    {
        physicalCollider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        Unlock(); // porta começa aberta por padrão
    }

    public void Lock()
    {
        isLocked = true;
        if (sr != null && closedSprite != null) sr.sprite = closedSprite; // já mostra fechada visualmente

        if (lockSound != null) AudioSource.PlayClipAtPoint(lockSound, transform.position);

        StopAllCoroutines();
        StartCoroutine(EnableColliderWhenClear());
    }

    private System.Collections.IEnumerator EnableColliderWhenClear()
    {
        DoorTrigger trigger = GetComponentInChildren<DoorTrigger>();

        // espera até o player não estar mais na área do vão
        while (trigger != null && trigger.playerInside)
        {
            yield return null;
        }

        physicalCollider.enabled = true;
    }


    public void Unlock()
    {
        isLocked = false;
        physicalCollider.enabled = false;
        if (sr != null && openSprite != null) sr.sprite = openSprite;

        if (unlockSound != null) AudioSource.PlayClipAtPoint(unlockSound, transform.position);
    }

    public bool IsLocked => isLocked;
}