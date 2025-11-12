using System.Collections;
using UnityEngine;

public class PlayerOutOfView : MonoBehaviour
{
    [SerializeField] private float flashDuration = 1f;
    [SerializeField] private float flashInterval = 0.1f;
    [SerializeField] private Renderer playerMeshRenderer;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is out of view, losing a life and respawning");

            HeartDisplay playerHealth = other.GetComponent<HeartDisplay>();
            if (playerHealth != null)
                playerHealth.TakeDamage();

            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

            if (playerMovement != null && playerMovement.anchorPoint != null)
            {
                other.transform.position = playerMovement.anchorPoint.transform.position;

                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.linearVelocity = Vector3.zero;
            }

            StartCoroutine(FlashPlayer(other.gameObject));
        }
    }

    private IEnumerator FlashPlayer(GameObject player)
    {
        if (playerMeshRenderer == null)
        {
            Debug.LogWarning("renderer is null!");
            yield break;
        }

        float elapsedTime = 0f;
        bool isMeshEnabled = true;

        while (elapsedTime < flashDuration)
        {
            isMeshEnabled = !isMeshEnabled;
            playerMeshRenderer.enabled = isMeshEnabled;
            elapsedTime += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }

        playerMeshRenderer.enabled = true;
    }
}
