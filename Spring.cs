using UnityEngine;

public class Spring : MonoBehaviour
{
    public float scaleDuration = 0.5f; // Duration to scale from 0 to 1
    public float upwardForce = 10f;     // Force applied to the object
    private bool isScaling = false;     // Check if the scaling is in progress
    private float targetScale = 1f;     // Target scale
    private float initialScale = 0f;     // Initial scale
    private float currentScale = 0f;    // Current scale

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // Start scaling the spring and apply the upward force
            if (!isScaling)
            {
                StartCoroutine(ScaleAndLaunch());
            }
        }
    }

    private System.Collections.IEnumerator ScaleAndLaunch()
    {
        isScaling = true;
        float elapsedTime = 0f;

        // Scale the object from 0 to 1 over the specified duration
        while (elapsedTime < scaleDuration)
        {
            currentScale = Mathf.Lerp(initialScale, targetScale, elapsedTime / scaleDuration);
            transform.localScale = new Vector3(1f, currentScale, 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set final scale
        transform.localScale = new Vector3(1f, targetScale, 1f);

        // Apply upward force
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply force upward
            rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
        }

        // Optionally reset scale after a while or destroy the object
        yield return new WaitForSeconds(1f); // Wait for a moment
        ResetScale();
    }

    private void ResetScale()
    {
        // Reset the scale back to zero
        transform.localScale = new Vector3(1f, 0f, 1f);
        isScaling = false;
    }
}
