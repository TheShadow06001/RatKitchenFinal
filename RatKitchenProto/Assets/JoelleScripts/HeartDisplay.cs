using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    public static HeartDisplay instance;
    public int health;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;
    public GameObject gameOverUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
                hearts[i].enabled = true; // make sure visible
            }
            else
            {
                hearts[i].enabled = false; // hide heart or removes it
            }

            if (i >= maxHealth)
            {
                hearts[i].enabled = false; // keep disabled if player has max health
            }
        }
    }

    public void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            health = 0;
            
            GameManager.Instance.SwitchState<DeathState>();

            //---G/O---
            if (gameOverUI != null)
                gameOverUI.SetActive(true);
            
            //Destroy player
            //GameObject player = GameObject.FindGameObjectWithTag("Player");
            //if(player !=null)
            //    Destroy(player);
                
            //Debug.Log("Player died, game over");
            /*Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name); */
            // player.transform.position = respawnPoint.position;
        }
        
    }
}
