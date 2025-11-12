using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private Animator playerAnimator;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                if (playerAnimator != null) playerAnimator.speed = 0f;
                pauseMenuUI.SetActive(true);
                SoundManager.Instance.PlaySoundEffect(SoundEffects.OpenPause);
                GameManager.Instance.SwitchState<PauseState>();
                isPaused = true;
            }
            else if (isPaused)
            {
                SoundManager.Instance.PlaySoundEffect(SoundEffects.ClosePause);
                GameManager.Instance.SwitchState<PlayingState>();
                isPaused = false;
                optionsUI.SetActive(false);
                pauseMenuUI.SetActive(false);

                if (playerAnimator != null)
                    playerAnimator.speed = 1f;
            }
        }
    }

    public void Resume()
    {
        SoundManager.Instance.PlaySoundEffect(SoundEffects.ClosePause);
        GameManager.Instance.SwitchState<PlayingState>();
        isPaused = false;
    }
}