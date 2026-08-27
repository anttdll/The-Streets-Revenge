using UnityEngine;

public class GameStart : MonoBehaviour
{
    public Room startingRoom;

    private void Start()
    {
        if (startingRoom != null)
        {
            CameraController.Instance.SetCurrentRoom(startingRoom);
            startingRoom.OnPlayerEnter();
        }
    }
}