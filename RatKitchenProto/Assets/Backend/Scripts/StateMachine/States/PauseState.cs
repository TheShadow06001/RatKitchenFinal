using UnityEngine;

public class PauseState : State
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionMenuUI;
    [SerializeField] private Animator playerAnimator;

    public override void EnterState()
    {
        base.EnterState();

        pauseMenuUI.SetActive(true);

        if (playerAnimator != null)
            playerAnimator.speed = 0f;

    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        optionMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);

        if (playerAnimator != null)
            playerAnimator.speed = 1f;
    }
}