using UnityEngine;

public class S_PauseMenuReturn : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        S_LevelManager.Instance.LoadLevel("Main Menu");
    }

    public void StartGame()
    {

        S_LevelManager.Instance.LoadLevel("GameLevel");
    }
}