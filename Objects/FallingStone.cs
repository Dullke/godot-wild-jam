using Godot;

public partial class FallingStone : CharacterBody3D
{
	[Export] bool isShiny;
    [Export] Area3D detection;

    public override void _EnterTree()
    {
        detection.BodyEntered += (a) => QueueFree();
    }

    public override void _Process(double delta)
	{
        Velocity += GetGravity() * (float)delta;
        MoveAndSlide();
	}
    
}
