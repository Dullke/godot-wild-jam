using Godot;


public partial class GolemWanderState : Node, GolemState
{
    [Export] public float wanderSpeed;
    [Export] public float attackCooldown;

    public bool startedFlag = false;
    private SceneTreeTimer attackTimer;

    public async void OnEnter(GolemStateMachine machine)
    {
        if (startedFlag) { }

        machine.animator.Play("Forward");

        await ToSignal(GetTree().CreateTimer(attackCooldown), SceneTreeTimer.SignalName.Timeout);
        machine.ChangeState("Attack");
    }


    public void OnTick(GolemStateMachine machine)
    {
        Vector3 playerPosition = ManagersDB.GameManager.player.GlobalPosition;
        Vector3 directionToPlayer = (playerPosition - machine.GlobalPosition).Normalized();

        machine.Velocity = directionToPlayer * wanderSpeed * ManagersDB.GameManager.GlobalDeltaTime;
        machine.MoveAndSlide();

        float angleToCursor = Mathf.Atan2(directionToPlayer.X, directionToPlayer.Z);
        machine.Rotation = Vector3.Up * Mathf.LerpAngle(machine.Rotation.Y, angleToCursor, ManagersDB.GameManager.GlobalDeltaTime);
    }

    public void OnExit(GolemStateMachine machine)
    {
    }
}
