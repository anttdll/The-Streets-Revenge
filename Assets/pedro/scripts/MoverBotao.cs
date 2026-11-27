using UnityEngine;
using UnityEngine.EventSystems;

public class MexerBotao1 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
        private Vector3 escalaOriginal;
        public float escalaHover = 1.1f;
        public float velocidade = 8f;

        private Vector3 escalaAlvo;

        void Start()
        {
            escalaOriginal = transform.localScale;
            escalaAlvo = escalaOriginal;
        }

        void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, escalaAlvo, Time.deltaTime * velocidade);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = escalaOriginal * 1.1f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = escalaOriginal;
        }
}
