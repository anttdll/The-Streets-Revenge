using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs de inimigos possíveis")]
    public GameObject[] enemyPrefabs;

    [Header("Pontos de spawn (filhos vazios nessa sala)")]
    public Transform[] spawnPoints;

    [Header("Quantidade")]
    public int minEnemies = 2;
    public int maxEnemies = 4;

    private Room room;

    void Awake()
    {
        room = GetComponent<Room>();
    }

    void Start()
    {
        SpawnRandomEnemies();
    }

    void SpawnRandomEnemies()
    {
        int count = Random.Range(minEnemies, maxEnemies + 1);
        count = Mathf.Min(count, spawnPoints.Length); // não passa do número de pontos disponíveis

        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < count; i++)
        {
            int pointIndex = Random.Range(0, availablePoints.Count);
            Transform spawnPos = availablePoints[pointIndex];
            availablePoints.RemoveAt(pointIndex); // evita spawnar 2 inimigos no mesmo ponto

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemyInstance = Instantiate(prefab, spawnPos.position, Quaternion.identity, transform);

            // registra o inimigo na sala (funciona pra qualquer tipo)
            Inimigo inimigoComum = enemyInstance.GetComponent<Inimigo>();
            if (inimigoComum != null)
            {
                inimigoComum.parentRoom = room;
                room.enemies.Add(enemyInstance);
                Debug.Log("Adicionado inimigo. Total agora: " + room.enemies.Count);
            }

            InimigoAtirador atirador = enemyInstance.GetComponent<InimigoAtirador>();
            if (atirador != null)
            {
                atirador.parentRoom = room;
                room.enemies.Add(enemyInstance);
                Debug.Log("Adicionado inimigo. Total agora: " + room.enemies.Count);
            }
        }

        if (room.enemies.Count == 0)
        {
            room.ForceMarkCleared();
        }
    }
}