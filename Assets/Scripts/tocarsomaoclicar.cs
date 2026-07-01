using UnityEngine;

public class TocarSomAoClicar : MonoBehaviour
{
    private AudioSource audioSource;
    private float cooldownTime = 0.7f;
    private float nextPlayTime = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        if (audioSource != null && Time.time >= nextPlayTime)
        {
            audioSource.Play();
            nextPlayTime = Time.time + cooldownTime;
        }
    }
}
