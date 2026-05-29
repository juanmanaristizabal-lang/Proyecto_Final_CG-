using UnityEngine;

public class ItemEfecto : MonoBehaviour
{
    [Header("Prefab de partículas")]
    public GameObject collectParticles;

    [Header("Sonido")]
    public AudioClip collectSound;
    public float volume = 1f;

    public void Play()
    {
       
        if (collectParticles != null)
        {
            GameObject effect = Instantiate(
                collectParticles,
                transform.position,
                Quaternion.identity);

            
            Destroy(effect, 2f);
        }

        // Reproduce sonido
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(
                collectSound,
                transform.position,
                volume);
    }
}
