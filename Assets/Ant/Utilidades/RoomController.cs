using Cainos.PixelArtTopDown_Basic;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [Header("Portas da Sala")]
    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject doorLeft;
    public GameObject doorRight;

    [Header("Câmera")]
    public CameraFollow cameraFollow; // Script que criaremos no passo 4

    private BoxCollider2D roomBounds;
    private bool isRoomActive = false;
    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        roomBounds = GetComponent<BoxCollider2D>();

        // Procura todos os inimigos que já nasceram e estão dentro do espaço da sala
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Inimigo");
        foreach (GameObject enemy in allEnemies)
        {
            if (roomBounds.bounds.Contains(enemy.transform.position))
            {
                enemies.Add(enemy);
            }
        }
    }

    // Usaremos OnTriggerEnter2D mesmo com o colisor desativado? 
    // Não! Vamos usar OnTriggerEnter2D no Player, mas o Player vai detectar a sala.
    // Para isso, o Player precisa de um pequeno sensor. 
    // Vamos fazer isso no passo 4!

    public void EnterRoom()
    {
        if (isRoomActive) return;

        isRoomActive = true;

        // Trava a câmera na sala
        if (cameraFollow != null)
        {
            cameraFollow.SetBounds(roomBounds);
        }

        // Fecha as portas
        SetDoors(true);

        // Começa a checar inimigos
        InvokeRepeating(nameof(CheckEnemies), 0.5f, 0.2f);
    }

    void SetDoors(bool closed)
    {
        if (doorUp != null) doorUp.SetActive(closed);
        if (doorDown != null) doorDown.SetActive(closed);
        if (doorLeft != null) doorLeft.SetActive(closed);
        if (doorRight != null) doorRight.SetActive(closed);
    }

    void CheckEnemies()
    {
        enemies.RemoveAll(item => item == null);

        if (enemies.Count == 0)
        {
            CancelInvoke(nameof(CheckEnemies));
            SetDoors(false); // Abre as portas
            isRoomActive = false;
        }
    }

    // Função pública para o Player adicionar um inimigo recém-nascido à lista
    public void AddEnemy(GameObject enemy)
    {
        if (roomBounds.bounds.Contains(enemy.transform.position) && !enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }
}