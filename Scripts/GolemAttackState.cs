using Godot;

public partial class GolemAttackState : Node, GolemState
{
    const int attacks = 3;
    [Export] private float spawnRadius;
    [Export] private PackedScene rocks;
    [Export] private PackedScene shiningRock;
    [Export] private Node3D golem;

    private int activeStones = 0;

    public void OnEnter(GolemStateMachine machine)
    {
        if (machine.startedFlag == false)
        {
            machine.animator.Play("Stomp");
            return;
        }

        if (activeStones < 2)
        {
            machine.animator.Play("Stomp");
        } else if (machine.GlobalPosition.DistanceTo(ManagersDB.GameManager.player.GlobalPosition) < 8) {
            machine.animator.Play("Punch");
        }

    }

    public void OnTick(GolemStateMachine machine)
    {
        Vector3 directionToPlayer = (ManagersDB.GameManager.player.GlobalPosition - machine.GlobalPosition).Normalized();
        float angle = Mathf.Atan2(directionToPlayer.X, directionToPlayer.Z);
        machine.Rotation = Vector3.Up * Mathf.LerpAngle(machine.Rotation.Y, angle, ManagersDB.GameManager.GlobalDeltaTime * 6);
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
            Node3D fallingRock = null;

            //Decides which stone will be affected by stasis
            //if (i == shiningStoneIndex)
            //    fallingRock = shiningRock.Instantiate() as Node3D;
            //else 
            fallingRock = rocks.Instantiate() as Node3D;

            //Spawns it and alocates randomly
            ManagersDB.GameManager.AddChild(fallingRock);
            fallingRock.Owner = ManagersDB.GameManager;

            Vector3 randomPosition = Vector3.Zero;
            do
            {
                randomPosition = new Vector3((float)GD.RandRange(-spawnRadius, spawnRadius), 25, (float)GD.RandRange(-spawnRadius, spawnRadius));
            } while (randomPosition.DistanceTo(golem.GlobalPosition) < 6);

            fallingRock.GlobalPosition = randomPosition;
        }
        activeStones = stonesAmount;
    }

}
