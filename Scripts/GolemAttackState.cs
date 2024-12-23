using Godot;
using Godot.Collections;

public partial class GolemAttackState : Node, GolemState
{
    const int attacks = 3;
    [Export] private float spawnRadius;
    [Export] private PackedScene rocks;
    [Export] private PackedScene shiningRock;
    [Export] private Node3D golem;
    [Export] private CollisionShape3D shape;

    private int punches;

    public void OnEnter(GolemStateMachine machine)
    {
        if (machine.startedFlag == false)
        {
            machine.animator.Play("Stomp");
            return;
        }


        if (punches > 3)
        {
            machine.animator.Play("Stomp");
            punches = 0;
        } else if (machine.GlobalPosition.DistanceTo(ManagersDB.GameManager.player.GlobalPosition) < 12) {
            machine.animator.Play("Punch");
            punches++;
        }
        else
        {
            machine.ChangeState("Wander");
        }
    }

    public void OnTick(GolemStateMachine machine, float fixedDeltaTime)
    {
        Vector3 directionToPlayer = (ManagersDB.GameManager.player.GlobalPosition - machine.GlobalPosition).Normalized();
        float angle = Mathf.Atan2(directionToPlayer.X, directionToPlayer.Z);
        machine.Rotation = Vector3.Up * Mathf.LerpAngle(machine.Rotation.Y, angle, ManagersDB.GameManager.GlobalDeltaTime * 10);
    }

    public void OnExit(GolemStateMachine machine)
    {
    }

    public void SpawnFallingStones()
    {
        int stonesAmount = GD.RandRange(2, 6);
        int shiningStoneIndex = GD.RandRange(0, stonesAmount - 1);
        for (int i = 0; i < stonesAmount; i++)
        {
            Node3D fallingRock = rocks.Instantiate() as Node3D;

            //Spawns it and alocates randomly
            ManagersDB.GameManager.AddChild(fallingRock);
            fallingRock.Owner = ManagersDB.GameManager;

            Vector3 randomPosition = Vector3.Zero;
            do
            {
                randomPosition = new Vector3((float)GD.RandRange(-spawnRadius, spawnRadius), 0, (float)GD.RandRange(-spawnRadius, spawnRadius));
            } while (randomPosition.DistanceTo(golem.GlobalPosition) < 6);

            randomPosition.Y = 25f;
            fallingRock.GlobalPosition = randomPosition;
        }
    }

}
