using UnityEngine;

public class StartSwitchScene : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.SwitchState<PlayingState>();
    }
}
