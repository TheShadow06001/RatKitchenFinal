public class PlayingState : State
{
    private S_TimerAndScore TimerAndScore;
    private KitchenGenerator kitchenGenerator;
    private CameraScript cameraScript;
    private PlayerMovement playerMovement;

    private void Awake()
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        TimerAndScore = FindFirstObjectByType<S_TimerAndScore>();
        kitchenGenerator = FindFirstObjectByType<KitchenGenerator>();
        cameraScript = FindFirstObjectByType<CameraScript>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
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
        kitchenGenerator.UpdateKitchenGenerator();
    }
}