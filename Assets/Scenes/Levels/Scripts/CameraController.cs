using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [Header("Referências")]
    public Transform player;

    [Header("Transição entre salas")]
    public bool useSmoothTransition = true;
    public float transitionSpeed = 8f;

    public Room CurrentRoom { get; private set; }

    private Camera cam;
    private Bounds currentBounds;
    private bool isTransitioning = false;
    private bool hasRoom = false;

    private void Awake()
    {
        Instance = this;
        cam = GetComponent<Camera>();
    }

    public void SetCurrentRoom(Room room)
    {
        CurrentRoom = room;
        currentBounds = room.roomBounds;
        hasRoom = true;
        isTransitioning = useSmoothTransition;

        FitCameraToRoom();
    }

    private void FitCameraToRoom()
    {
        float roomHeight = currentBounds.size.y;
        float roomWidth = currentBounds.size.x;

        float sizeByHeight = roomHeight / 2f;
        float sizeByWidth = roomWidth / (2f * cam.aspect);

        cam.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);
    }

    private void LateUpdate()
    {
        if (!hasRoom || player == null) return;

        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        Vector3 desired = player.position;

        float minX = currentBounds.min.x + camHalfWidth;
        float maxX = currentBounds.max.x - camHalfWidth;
        float minY = currentBounds.min.y + camHalfHeight;
        float maxY = currentBounds.max.y - camHalfHeight;

        desired.x = (minX <= maxX) ? Mathf.Clamp(desired.x, minX, maxX) : currentBounds.center.x;
        desired.y = (minY <= maxY) ? Mathf.Clamp(desired.y, minY, maxY) : currentBounds.center.y;
        desired.z = transform.position.z;

        if (isTransitioning)
        {
            transform.position = Vector3.Lerp(transform.position, desired, transitionSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, desired) < 0.05f)
                isTransitioning = false;
        }
        else
        {
            transform.position = desired;
        }
    }
}