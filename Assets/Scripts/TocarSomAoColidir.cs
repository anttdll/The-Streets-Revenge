using UnityEngine;

public class TocarSomAoColidir : MonoBehaviour
{
    private AudioSource somDoObjeto;

    void Start()
    {
        // Pega o componente Audio Source anexado ao objeto
        somDoObjeto = GetComponent<AudioSource>();
    }

    // Exemplo: Toca o som quando algo colide com este objeto
    private void OnCollisionEnter(Collision collision)
    {
       
        if (somDoObjeto != null)
        {
            somDoObjeto.Play();
        }
    }
}
