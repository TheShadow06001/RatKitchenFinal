using System.Threading;
using UnityEngine;

public class HP : MonoBehaviour
{

    private int health = 2;
    [SerializeField] private float regenTime = 3f;
    [SerializeField] private GameObject gameOverMenu;
    private float timer;
    


    private void Update()
    {
        if (GameManager.Instance.CheckState<PlayingState>())
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
                
            }
            else if (timer <= 0)
            {
                RegenHealth();
            }

            
        }
    }

    private void RegenHealth()
    {
        health++;
    }

    public void TakeDamage(int damage)
    {

        health -= damage;
        timer = regenTime;

        if (health <= 0)
        {
            Debug.Log("Game Over");
            if (gameOverMenu != null)
            {
                gameOverMenu.SetActive(true);
            }
        }
    }
}
