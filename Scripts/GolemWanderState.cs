using Godot;

public partial class GolemWanderState : Node, GolemState
{
    [Export] public float wanderSpeed;
    [Export] public Timer attackCooldown;

    public bool startedFlag = false;


    public  void OnEnter(GolemStateMachine machine)
    {
        machine.animator.Play("Forward");
        attackCooldown.Timeout += () => machine.ChangeState("Attack");
        attackCooldown.Start();
    }


    public void OnTick(GolemStateMachine machine, float fixedDeltaTime)
    {
        Vector3 playerPosition = ManagersDB.GameManager.player.GlobalPosition;
        Vector3 directionToPlayer = (playerPosition - machine.GlobalPosition).Normalized();

        machine.Velocity = directionToPlayer * wanderSpeed * fixedDeltaTime;
        if (ManagersDB.GameManager.TimeFrozenFlag == false) machine.MoveAndSlide();

        float angleToCursor = Mathf.Atan2(directionToPlayer.X, directionToPlayer.Z);
        machine.Rotation = Vector3.Up * Mathf.LerpAngle(machine.Rotation.Y, angleToCursor, ManagersDB.GameManager.GlobalDeltaTime * 4);
    }

    public void OnExit(GolemStateMachine machine)
    {
        attackCooldown.Stop();
    }

}
