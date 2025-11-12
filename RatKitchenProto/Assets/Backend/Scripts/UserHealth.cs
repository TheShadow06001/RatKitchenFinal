using UnityEngine;

public class UserHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 5;

    private void Start()
    {
        health = maxHealth;

    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }

    /* private void RespawnHere()
    {
        transform.position = respawnPosition;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("Player is out of lives");
    }*/
}
