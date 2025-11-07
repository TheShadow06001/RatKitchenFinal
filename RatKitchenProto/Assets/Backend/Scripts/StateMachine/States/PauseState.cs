using UnityEngine;

public class PauseState : State
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionMenuUI;

    public override void EnterState()
    {
        pauseMenuUI.SetActive(true);
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        optionMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
    }
}
