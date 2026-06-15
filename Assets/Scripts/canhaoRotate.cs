using UnityEngine;

public class canhaoRotate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, 90 * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, -90 * Time.deltaTime);
        }
        Mathf.Clamp(transform.rotation.z, -90, 90);
    }
}


