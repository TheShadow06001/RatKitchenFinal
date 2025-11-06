using UnityEngine;

public class RatHoleTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // GameManager.Instance.NextLevel();
            // ResetKitchenGenerator();
            // Fade?
            // Respawn
            Debug.Log("Player entered rathole, load next level");
            DifficultyManager.Instance.LevelComplete();

        }
    }
}
