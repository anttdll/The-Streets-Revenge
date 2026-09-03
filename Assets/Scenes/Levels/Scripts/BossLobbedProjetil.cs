using UnityEngine;

public class BossLobbedProjetil : MonoBehaviour
{
    [Header("Configuração do arco")]
    public float arcHeight = 3f;
    public float travelTime = 1.2f;
    public int damage = 1;
    public float damageRadius = 1.2f;

    [Header("Visual de aviso")]
    public GameObject warningIndicatorPrefab; // um círculo simples (Sprite) mostrando onde vai cair

    private Vector3 startPos;
    private Vector3 targetPos;
    private float timer = 0f;
    private SpriteRenderer sr;

    public void Launch(Vector3 from, Vector3 to)
    {
        startPos = from;
        targetPos = to;
        transform.position = startPos;

        sr = GetComponent<SpriteRenderer>();

        if (warningIndicatorPrefab != null)
        {
            GameObject warning = Instantiate(warningIndicatorPrefab, targetPos, Quaternion.identity);
            Destroy(warning, travelTime);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / travelTime;

        if (t >= 1f)
        {
            Land();
            return;
        }

        // interpola posição linear no plano + curva de altura (efeito de arco)
        Vector3 flatPos = Vector3.Lerp(startPos, targetPos, t);
        float height = arcHeight * Mathf.Sin(t * Mathf.PI); // sobe e desce suavemente
        transform.position = flatPos + new Vector3(0, height * 0.3f, 0); // 0.3f só pra achatar visualmente em 2D top-down

        // opcional: escala leve pra simular profundidade
        transform.localScale = Vector3.one * (1f + height * 0.15f);
    }

    void Land()
    {
        transform.position = targetPos;

        Collider2D[] hits = Physics2D.OverlapCircleAll(targetPos, damageRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth ph = hit.GetComponent<PlayerHealth>();
                if (ph != null) ph.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}