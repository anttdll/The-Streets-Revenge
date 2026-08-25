using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject gatoPrefab;
    public float fireRate = 0.15f;
    public Transform firePoint;

    [Header("Áudio")]
    public AudioClip meowSound;

    private float fireTimer = 0f;
    private Vector2 shootDirection = Vector2.down;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (firePoint == null)
        {
            GameObject fp = new GameObject("FirePoint");
            fp.transform.parent = transform;
            fp.transform.localPosition = Vector3.zero;
            firePoint = fp.transform;
        }
    }

    void Update()
    {
        // Diminui o timer
        if (fireTimer > 0)
            fireTimer -= Time.deltaTime;

        // Verifica setas
        Vector2 shootInput = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow)) shootInput = Vector2.up;
        else if (Input.GetKey(KeyCode.DownArrow)) shootInput = Vector2.down;
        else if (Input.GetKey(KeyCode.LeftArrow)) shootInput = Vector2.left;
        else if (Input.GetKey(KeyCode.RightArrow)) shootInput = Vector2.right;

        // Atira se tiver seta e timer zerado
        if (shootInput != Vector2.zero && fireTimer <= 0f && gatoPrefab != null)
        {
            shootDirection = shootInput;
            Shoot(shootDirection);
            fireTimer = fireRate;

            // ===== ATIVA ANIMAÇÃO DE ATAQUE =====
            if (playerController != null)
            {
                playerController.PlayAttackAnimation();
            }
        }
    }

    void Shoot(Vector2 direction)
    {
        if (gatoPrefab == null || firePoint == null) return;

        GameObject gato = Instantiate(gatoPrefab, firePoint.position, Quaternion.identity);
        GatoProjetil projetil = gato.GetComponent<GatoProjetil>();

        if (projetil != null)
        {
            projetil.Launch(direction);
        }

        if (meowSound != null)
        {
            AudioSource.PlayClipAtPoint(meowSound, transform.position);
        }
    }
}