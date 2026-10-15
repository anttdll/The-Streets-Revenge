using UnityEngine;

public class HpDrop : MonoBehaviour
{
    public GameObject hpDropPrefab;

    [Range(0f, 1f)]
    public float dropChance = 0.3f; // 1 = sempre dropa, 0.3 = 30%

    public void Dropar()
    {
        if (hpDropPrefab == null) return;
        if (Random.value > dropChance) return;

        Instantiate(hpDropPrefab, transform.position, Quaternion.identity);
    }
}   