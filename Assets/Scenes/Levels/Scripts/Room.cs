using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Room : MonoBehaviour
{
    [HideInInspector] public Bounds roomBounds;

    [Header("Inimigos da sala")]
    public List<GameObject> enemies = new List<GameObject>(); // arraste os inimigos no Inspector

    [Header("Portas da sala")]
    public List<Door> doors = new List<Door>(); // arraste as portas no Inspector

    private bool roomCleared = false;
    private bool playerEntered = false;
    public bool IsActive { get; private set; } = true; // salas sem trava começam ativas

    private void Awake()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.isTrigger = true;
        roomBounds = box.bounds;

        // remove da lista qualquer inimigo nulo (segurança)
        enemies.RemoveAll(e => e == null);
        roomCleared = enemies.Count == 0;
    }

    // chamado pelo RoomTrigger quando o player entra na sala
    public void OnPlayerEnter()
    {
        if (playerEntered) return;
        playerEntered = true;

        if (!roomCleared && enemies.Count > 0)
        {
            IsActive = false;
            StartCoroutine(ActivateRoomDelayed());
            StartCoroutine(LockDoorsDelayed());
        }
    }

    private System.Collections.IEnumerator ActivateRoomDelayed()
    {
        yield return new WaitForSeconds(0.5f);
        IsActive = true;
    }

    private System.Collections.IEnumerator LockDoorsDelayed()
    {
        yield return new WaitForSeconds(0.3f); // tempo pro player atravessar a porta
        LockDoors();
    }
    // chamado pelo inimigo (via EnemyHealth) quando ele morre
    public void NotifyEnemyDefeated(GameObject enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0 && !roomCleared)
        {
            roomCleared = true;
            UnlockDoors();
        }
    }

    private void LockDoors()
    {
        foreach (Door door in doors)
        {
            if (door != null) door.Lock();
        }
    }

    private void UnlockDoors()
    {
        foreach (Door door in doors)
        {
            if (door != null) door.Unlock();
        }
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box == null) return;
        Gizmos.color = roomCleared ? Color.green : Color.cyan;
        Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);


    }

    
}