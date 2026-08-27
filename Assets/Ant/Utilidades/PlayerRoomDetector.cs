using UnityEngine;

public class PlayerRoomDetector : MonoBehaviour
{
    private RoomController currentRoom;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Se o objeto que colidiu tem o script RoomController
        RoomController room = other.GetComponent<RoomController>();
        if (room != null)
        {
            currentRoom = room;
            currentRoom.EnterRoom();
        }
    }
}