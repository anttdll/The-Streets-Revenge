using Unity.VisualScripting;
using UnityEngine;

public class destruit : MonoBehaviour
{
    [SerializeField]public static int pegarItem;
    [SerializeField] void OnCollisionEnter2D(Collision2D collision)
        {


        Destroy(gameObject);
        pegarItem++;


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


