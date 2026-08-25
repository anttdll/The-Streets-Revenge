using UnityEngine;

public class Canhao : MonoBehaviour
{
    public Rigidbody2D projetil;
    public float velocidade;
    public float timeBetween = 0.7f;
    public Transform direction;
    private float nextFireTime = 0.7f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            // Só atira se tiver gato disponível
            if (destruit.pegarItem > 0)
            {
                Fire();

                // Gastou 1 gato
                destruit.pegarItem--;

                nextFireTime = Time.time + timeBetween;
            }
        }
    }

    void Fire()
    {
        Rigidbody2D rb = Instantiate(
            projetil,
            transform.position,
            direction.transform.rotation
        );

        rb.linearVelocity = direction.transform.right * velocidade;
    }
}