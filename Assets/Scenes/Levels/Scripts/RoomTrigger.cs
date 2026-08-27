using UnityEngine;

[RequireComponent(typeof(Room))]
public class RoomTrigger : MonoBehaviour
{
    private Room room;

    private void Awake()
    {
        room = GetComponent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entrou na sala: " + room.name);
            CameraController.Instance.SetCurrentRoom(room);
            room.OnPlayerEnter();
        }
    }


}