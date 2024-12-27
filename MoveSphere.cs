using UnityEngine;

public class OscillateSphere : MonoBehaviour
{
    public float speed = 2f;  // Speed of the oscillation
    public float distance = 3f; // Maximum distance to move forward and backward
    public float rotationSpeed = 100f; // Speed of the sphere's rotation

    private Vector3 startPosition;

    void Start()
    {
        // Store the starting position of the sphere
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new z position using a sine wave to oscillate
        float zOffset = Mathf.Sin(Time.time * speed) * distance;

        // Apply the new position to the sphere
        transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z + zOffset);

        // Rotate the sphere around the Y-axis (or any other axis as needed)
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
