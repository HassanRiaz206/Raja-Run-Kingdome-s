using UnityEngine;
using System.Collections;
public class RespawnPlayer : MonoBehaviour
{
    public Transform source;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(RespawnAfterDelay(other.gameObject));
        }
    }

    private IEnumerator RespawnAfterDelay(GameObject player)
    {
        yield return new WaitForSeconds(0.5f);
        player.transform.position = source.position;
    }
}
