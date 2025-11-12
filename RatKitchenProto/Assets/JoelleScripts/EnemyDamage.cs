using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
<<<<<<< Updated upstream
   
     
=======
    public GameObject PlayerHeartDisplay;
    public int damage = 2;

>>>>>>> Stashed changes
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
        if (collision.gameObject.CompareTag("Player"))
        {
            HeartDisplay.instance.TakeDamage();
        }
    }
}



