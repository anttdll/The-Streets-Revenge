using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public enum DoorDirection { Horizontal, Vertical }

    public Room roomA;
    public Room roomB;
    public DoorDirection direction;
    public float edgeMargin = 1f;

    [HideInInspector] public bool playerInside = false;

    private bool isTeleporting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;

        if (isTeleporting) return;

        Room current = CameraController.Instance.CurrentRoom;
        Room target = null;

        if (current == roomA) target = roomB;
        else if (current == roomB) target = roomA;

        if (target != null)
        {
            TeleportPlayer(other.transform, target);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private void TeleportPlayer(Transform player, Room target)
    {
        isTeleporting = true;

        Bounds targetBounds = target.roomBounds;
        Vector3 playerPos = player.position;
        Vector3 newPos = playerPos;

        if (direction == DoorDirection.Horizontal)
        {
            bool enteringFromLeft = playerPos.x < transform.position.x;
            newPos.x = enteringFromLeft
                ? targetBounds.min.x + edgeMargin
                : targetBounds.max.x - edgeMargin;
        }
        else
        {
            bool enteringFromBelow = playerPos.y < transform.position.y;
            newPos.y = enteringFromBelow
                ? targetBounds.min.y + edgeMargin
                : targetBounds.max.y - edgeMargin;
        }

        player.position = newPos;

        CameraController.Instance.SetCurrentRoom(target);
        target.OnPlayerEnter();

        playerInside = false; // já foi teleportado pra longe, não está mais "dentro"
        isTeleporting = false;
    }
}