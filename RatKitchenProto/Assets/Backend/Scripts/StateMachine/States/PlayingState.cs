using UnityEngine;
using UnityEngine.UI;

public class PlayingState : State
{
    [SerializeField] private S_TimerAndScore TimerAndScore;
    [SerializeField] private KitchenGenerator KitchenGenerator;
    private PlayerMovement playerMovement;
    private CameraScript cameraScript;

    private void Awake()
    {
        cameraScript = FindFirstObjectByType<CameraScript>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
    
    public override void UpdateState()
    {
        base.UpdateState();

        cameraScript.UpdateCamera();
        playerMovement.PlayerUpdate();
        TimerAndScore.UpdateTimer();
        KitchenGenerator.UpdateKitchenGenerator();

        
    }
}
