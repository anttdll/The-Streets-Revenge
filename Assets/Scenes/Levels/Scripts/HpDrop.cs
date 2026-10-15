using UnityEngine;

public class HpDrop : MonoBehaviour
{
    public GameObject hpDropPrefab;

    [Range(0f, 1f)]
    public float dropChance = 0f; // 1 = sempre dropa, 0.3 = 30%

    // no Dropar() do HpDrop
    public float dropRadius = 0.5f;

    public void Dropar()
    {
        if (hpDropPrefab == null) return;
        if (Random.value > dropChance) return;

        Vector2 offset = Random.insideUnitCircle * dropRadius;
        Instantiate(hpDropPrefab, (Vector2)transform.position + offset, Quaternion.identity);
    }
}