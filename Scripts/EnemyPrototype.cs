using Godot;

public partial class EnemyPrototype : CharacterBody3D
{
    [Export] private float moveSpeed = 5.0f;

    public override void _PhysicsProcess(double delta)
    {
        Vector3 playerPosition = ManagersDB.GameManager.player.GlobalPosition;
        Vector3 directionToPlayer = (playerPosition - GlobalPosition).Normalized();

        Velocity = directionToPlayer * moveSpeed * ManagersDB.GameManager.GlobalDeltaTime;
        MoveAndSlide();
    }
}
