using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    public static HeartDisplay instance;
    public int health;
    public int maxHealth;
    
    public Sprite fullHeart;
    public Image[] hearts;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
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
                hearts[i].enabled = true; 
            }
            else
            {
                hearts[i].enabled = false; // hide heart instead of showing empty one
            }

            if (i >= maxHealth)
            {
                hearts[i].enabled = false; 
            }
        }
    }

    
    public void TakeDamage()
    {
        health--;

        if (health > 0)
        {
            Debug.Log("Player died, game over");
            /*Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name); */
            // player.transform.position = respawnPoint.position;
        }
        
    }
}
