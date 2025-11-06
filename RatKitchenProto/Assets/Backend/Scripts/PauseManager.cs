using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (!isPaused)
            {
                SoundManager.Instance.PlaySoundEffect(SoundEffects.OpenPause);
                GameManager.Instance.SwitchState<PauseState>();
                isPaused = true;
            }
            else if (isPaused)
            {
                SoundManager.Instance.PlaySoundEffect(SoundEffects.ClosePause);
                GameManager.Instance.SwitchState<PlayingState>();
                isPaused = false;
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
