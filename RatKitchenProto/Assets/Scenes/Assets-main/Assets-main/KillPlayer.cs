using System.Collections;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{




    private bool canKill = true;

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canKill) return;

        if (other.gameObject.CompareTag("Player"))
        {
            HealthDisplay.instance.TakeDamage();

        }
    }

    IEnumerator RespawnDelay()
    {
        canKill = false; // disable killing temporarily

        yield return new WaitForSeconds(1f); // 1 second of invulnerability
        canKill = true; // re-enable after delay
    }
}
