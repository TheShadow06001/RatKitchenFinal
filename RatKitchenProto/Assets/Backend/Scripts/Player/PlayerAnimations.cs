using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] float jumpDuration;
    [SerializeField] private float RunSpeed;
    public void TriggerJump()
    {
        playerAnimator.SetTrigger("Jump");
    }
}
