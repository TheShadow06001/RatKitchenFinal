using UnityEngine;

public class S_GameOverMenuButtonScript : MonoBehaviour
{
    public void RestartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLevel");
    }
    
    public void MainMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
