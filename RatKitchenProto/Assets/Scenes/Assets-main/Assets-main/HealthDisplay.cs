using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public static HealthDisplay instance;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gameOverMenu;
    public int health;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    private void Awake()
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

        if (gameOverMenu != null) gameOverMenu.SetActive(false);
    }


    // Update is called once per frame
    private void Update()
    {
        for (var i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
            if (i < maxHealth)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }

    public void TakeDamage()
    {
        health--;

        if (health > 0)
        {
            /*Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name); */
            player.transform.position = respawnPoint.position;
        }
        else
        {
            health = 0;
            Debug.Log("Game Over");
            if (gameOverMenu != null) gameOverMenu.SetActive(true);
        }
    }
}