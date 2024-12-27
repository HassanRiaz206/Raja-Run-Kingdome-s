using UnityEngine;

public class CollisionParticleEffect : MonoBehaviour
{
    // Reference to the particle effect prefabs
    public GameObject particleEffect1;
    public GameObject particleEffect2;

    // Position where the particle effects should be spawned
    public Transform spawnPosition;

    // Tag of the Intro object
    public string introTag = "Intro";

    // When the Player collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the specified Intro tag
        if (collision.gameObject.CompareTag(introTag))
        {
            // Instantiate the first particle effect at the spawn position
            Instantiate(particleEffect1, spawnPosition.position, Quaternion.identity);

            // Instantiate the second particle effect at the spawn position
            Instantiate(particleEffect2, spawnPosition.position, Quaternion.identity);

            // Optional: Destroy the Intro object after collision
            // Destroy(collision.gameObject);
        }
    }
}
