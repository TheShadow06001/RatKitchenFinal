using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public UserHealth userHealth;
    public int damage = 2;
     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            userHealth.TakeDamage(damage);
        }
    }
}



