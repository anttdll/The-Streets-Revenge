using TMPro;
using UnityEngine;

public class Coletor : MonoBehaviour
{
        public TMP_Text Contador;
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        Contador.text = "Gatos Coletados: " + destruit.pegarItem;
    }
}
