using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoMexer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 escalaOriginal;
    private Vector3 escalaAlvo;

    public float multiplicadorEscala = 1.1f;
    public float velocidadeTransicao = 15f;

    void Start()
    {
        escalaOriginal = transform.localScale;
        escalaAlvo = escalaOriginal;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, escalaAlvo, Time.deltaTime * velocidadeTransicao);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        escalaAlvo = escalaOriginal * multiplicadorEscala;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        escalaAlvo = escalaOriginal;
    }
}
