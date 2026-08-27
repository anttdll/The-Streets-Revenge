using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Não precisamos mais de Target para seguir, precisamos apenas travar na sala.
    // Mas se você quiser, pode manter o Target para calcular o centro no inicio.

    private Bounds cameraBounds;
    private bool hasBounds = false;

    // Velocidade da transição (se quiser que seja instantânea, use 1.0f ou muito alto)
    [Range(0.1f, 1.0f)]
    public float transitionSpeed = 0.5f;

    void LateUpdate()
    {
        if (!hasBounds) return;

        // Posição desejada é SEMPRE o centro da sala (estilo TBOI)
        Vector3 desiredPosition = new Vector3(cameraBounds.center.x, cameraBounds.center.y, -10);

        // Move a câmera em direção ao centro da sala de forma suave
        transform.position = Vector3.Lerp(transform.position, desiredPosition, transitionSpeed);
    }

    // Método chamado pelo RoomController quando o player entra na sala
    public void SetBounds(BoxCollider2D roomCollider)
    {
        cameraBounds = roomCollider.bounds;
        hasBounds = true;
    }
}