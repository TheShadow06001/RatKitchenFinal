using System.Collections;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    [SerializeField] private int damage = 1;       
    [SerializeField] private float damageCooldown = 1f; 

    private bool canDamage = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;

        if (other.CompareTag("Player"))
        {
            
            UserHealth health = other.GetComponent<UserHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
