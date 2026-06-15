using UnityEngine;

public class TriggerParticulas : MonoBehaviour
{
    // Arraste o seu Sistema de Partículas para este campo na Unity
    public ParticleSystem particaEfeito;

    // Se estiver usando 3D, mude para OnTriggerEnter e Collider
    private void OnTriggerEnter2D(Collider2D outro)
    {
        // Verifique se o objeto que tocou é o jogador (opcional, mas recomendado)
        if (outro.CompareTag("Player"))
        {
            // Ativa o sistema de partículas
            if (particaEfeito != null)
            {
                particaEfeito.Play();
            }
        }
    }
}