using UnityEngine;

public class S_PauseMenuReturn : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        S_LevelManager.Instance.LoadLevel("Main Menu");
    }

    public void StartGame()
    {
        S_LevelManager.Instance.LoadLevel("GameLevel");
    }
}