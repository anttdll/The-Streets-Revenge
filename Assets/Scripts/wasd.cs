using UnityEngine;



public class wasd : MonoBehaviour



{
    private float Horizontal;
    private float Vertical;



    Vector2 movement;



    int speed = 2-0;
    private Rigidbody2D _rb;





    void Start()



    {
        _rb = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame



    void Update()



    {
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");



        _rb.linearVelocity = new Vector2(_horizontal * speed, _vertical * speed).normalized;








    }
    }



