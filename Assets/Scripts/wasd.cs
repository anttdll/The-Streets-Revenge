using UnityEngine;

public class wasd : MonoBehaviour

{

    int speed = 5;

    void Start()

    {

    }

    // Update is called once per frame

    void Update()

    {

        float moveX = 0;

        float moveY = 0;

        if (Input.GetKey(KeyCode.A))

        {

            moveX = -1;

        }

        if (Input.GetKey(KeyCode.D))

        {

            moveX = 1;

        }

        if (Input.GetKey(KeyCode.W))

        {

            moveY = 1;

        }

        if ((Input.GetKey(KeyCode.S)))

        {

            moveY = -1;

        }

       

        Vector3 movement = new Vector3(moveX, moveY, 0f).normalized;

        transform.position += movement * speed * Time.deltaTime;



    }

}

