using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Tooltip("As duas salas que esta porta conecta")]
    public Room roomA;
    public Room roomB;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Room current = CameraController.Instance.CurrentRoom;
        Room target = null;

        // decide pra qual sala ir baseado em onde o player está agora
        if (current == roomA) target = roomB;
        else if (current == roomB) target = roomA;

        if (target != null)
        {
            Debug.Log("[DoorTrigger] Indo de " + current.name + " para " + target.name);
            CameraController.Instance.SetCurrentRoom(target);
            target.OnPlayerEnter();
        }
    }
}