using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{


    public class DialogueLine : MonoBehaviour
    {
    private Text textHolder;
    [SerializeField] private string input;

        private void Awake()
        {
            textHolder = GetComponent<Text>();

            StartCoroutine(WriteText(input, textHolder));
        }
    }
}
