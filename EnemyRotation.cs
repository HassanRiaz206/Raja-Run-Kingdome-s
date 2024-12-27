using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    // Speed of rotation in degrees per second
    public float rotationSpeed = 45f;

    void Update()
    {
        // Calculate the amount of rotation for this frame
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Rotate around the Y-axis
        transform.Rotate(Vector3.up, rotationAmount);
    }
}
