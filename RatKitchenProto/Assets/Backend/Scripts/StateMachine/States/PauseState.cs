using Unity.VisualScripting;
using UnityEngine;

public class PauseState : State
{
    [SerializeField] private GameObject pauseMenuUI;

    public override void EnterState()
    {
        pauseMenuUI.SetActive(true);
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
       pauseMenuUI.SetActive(false);
    }
}
