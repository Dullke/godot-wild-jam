using Godot;

public partial class FallingStone : CharacterBody3D, IFreezable
{
	[Export] bool isShiny;
    [Export] Area3D detection;
    [Export] Node3D highlight;


    public void Highlight()
    {
        highlight.Visible = true;
        SetPhysicsProcess(false);
    }

    public void Unfreeze()
    {
        highlight.Visible = false;
        SetPhysicsProcess(true);
    }

    public override void _EnterTree()
    {
        detection.BodyEntered += (a) => QueueFree();
    }


    public override void _PhysicsProcess(double delta)
	{
        Velocity += GetGravity() * ManagersDB.GameManager.GlobalDeltaTime;
        if (ManagersDB.GameManager.TimeFrozenFlag == false) MoveAndSlide();
	}
    
}

public interface IFreezable
{
    public abstract void Highlight();

    public abstract void Unfreeze();
}
