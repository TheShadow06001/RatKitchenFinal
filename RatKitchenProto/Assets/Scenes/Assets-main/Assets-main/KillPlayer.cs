using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public GameObject player;
    public Transform respawnPoint;
    public GameObject gameOverMenu;

    private bool canKill = true;

    // Start is called before the first frame update
    private void Start()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (!canKill) return;

        if (other.gameObject.CompareTag("Player"))
        {

            HealthDisplay.instance.health--;

            if (HealthDisplay.instance.health > 0)
            {
                /*Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name); */
                player.transform.position = respawnPoint.position;
            }
            else
            {
                HealthDisplay.instance.health = 0;
                Debug.Log("Game Over");
                if (gameOverMenu != null)
                {
                    gameOverMenu.SetActive(true);
                }
            }
        }
    }

    IEnumerator RespawnDelay()
    {
        canKill = false; // disable killing temporarily
        player.transform.position = respawnPoint.position;
        yield return new WaitForSeconds(1f); // 1 second of invulnerability
        canKill = true; // re-enable after delay
    }
}
