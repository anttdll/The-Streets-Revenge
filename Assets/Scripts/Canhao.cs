using UnityEngine;

public class Canhao : MonoBehaviour
{
    public Rigidbody2D projetil;
    public float velocidade;

    public float timeBetween;
    public Transform direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBetween <= 0)
        {
            Fire();

            timeBetween = 2f;
        }
        else
        {
                       timeBetween -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Vector3 girar = new Vector3(0, 0, 90);
        }
    }

    void Fire()
    {
         Rigidbody2D rb = Instantiate(projetil, transform.position, direction.transform.rotation);

        rb.linearVelocity = transform.right * velocidade;
    }
}
