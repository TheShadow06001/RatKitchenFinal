using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    public static HeartDisplay instance;
    public int health;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(instance);
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
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
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

            GameManager.Instance.SwitchState<MenuState>();
            SceneManager.LoadScene("Main Menu");

            /*Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name); */
            // player.transform.position = respawnPoint.position;
        }

    }
}
