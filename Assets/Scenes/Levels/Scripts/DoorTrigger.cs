using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public enum DoorDirection { Horizontal, Vertical }

    [Tooltip("As duas salas que esta porta conecta")]
    public Room roomA;
    public Room roomB;

    [Tooltip("Horizontal = porta na parede esquerda/direita | Vertical = porta na parede topo/baixo")]
    public DoorDirection direction;

    [Tooltip("Distância de margem da borda da sala pra evitar reentrar no trigger")]
    public float edgeMargin = 1f;

    private bool isTeleporting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isTeleporting) return;

        Room current = CameraController.Instance.CurrentRoom;
        Room target = null;

        if (current == roomA) target = roomB;
        else if (current == roomB) target = roomA;

        if (target != null)
        {
            TeleportPlayer(other.transform, target);
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
            // decide se entra pela esquerda ou direita da nova sala
            bool enteringFromLeft = playerPos.x < transform.position.x;
            newPos.x = enteringFromLeft
                ? targetBounds.min.x + edgeMargin
                : targetBounds.max.x - edgeMargin;
            // mantém o Y (altura) igual à posição por onde o player atravessou
        }
        else // Vertical
        {
            bool enteringFromBelow = playerPos.y < transform.position.y;
            newPos.y = enteringFromBelow
                ? targetBounds.min.y + edgeMargin
                : targetBounds.max.y - edgeMargin;
            // mantém o X (largura) igual à posição por onde o player atravessou
        }

        player.position = newPos;

        CameraController.Instance.SetCurrentRoom(target);
        target.OnPlayerEnter();

        isTeleporting = false;
    }
}