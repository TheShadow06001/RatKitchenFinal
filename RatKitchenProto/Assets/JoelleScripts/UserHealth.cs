using UnityEngine;

public class UserHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 3;

    private Vector3 respawnPosition;

    private void Start()
    {
        health = maxHealth;
        respawnPosition = transform.position;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0); 

        if (health > 0)
        {
            RespawnHere();
        }
        else
        {
            Die();
        }
    }

    private void RespawnHere()
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
        Debug.Log("Player is out of lives!");
    }
}
