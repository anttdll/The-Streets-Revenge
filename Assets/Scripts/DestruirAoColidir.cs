using UnityEngine;

public class DestruirAoColidir : MonoBehaviour
{
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Inimigo"))
        {
           
            Destroy(gameObject);
        }
    }
}
