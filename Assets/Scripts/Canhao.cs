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
        //if (timebetween <= 0)
        //{
        //    fire();

        //    timebetween = 2f;
        //}
        //else
        //{
        //    timebetween -= time.deltatime;
        //}

        //if (input.getkey(keycode.a))
        //{
        //    vector3 girar = new vector3(0, 0, 90);
        //}


        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }


        void Fire()
        {
            Rigidbody2D rb = Instantiate(projetil, transform.position, direction.transform.rotation);

            rb.linearVelocity = direction.transform.right * velocidade;
        }
    }
}
