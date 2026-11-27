using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{


    public class DialogueLine : DialogueBaseClass
    {
    private Text textHolder;

        [Header("Dialogue Line Settings")]
        [SerializeField] private string input;
        [SerializeField] private Color textColor;
        [SerializeField] private Font textFont;

        [Header("Dialogue Line Timers")]
        [SerializeField] private float delay;
        private void Awake()
        {
            textHolder = GetComponent<Text>();

            StartCoroutine(WriteText(input, textHolder, textColor, textFont, delay));
        }
    }
}
