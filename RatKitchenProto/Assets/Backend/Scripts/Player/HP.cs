using UnityEngine;
using UnityEngine.SceneManagement;

public class HP : MonoBehaviour
{
    [SerializeField] private float regenTime = 3f;
    [SerializeField] private GameObject gameOverMenu;

    private int health = 2;
    private float timer;


    private void Update()
    {
        if (GameManager.Instance.CheckState<PlayingState>())
        {
            if (timer >= 0)
                timer -= Time.deltaTime;
            else if (timer <= 0) RegenHealth();
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
            SceneManager.LoadScene("Main Menu");

            Debug.Log("Game Over");
            if (gameOverMenu != null) gameOverMenu.SetActive(true);
        }
    }
}