using Godot;

public partial class FallingStone : Node3D, IFreezable
{
	[Export] bool isShiny;
    [Export] Area3D detection;
    [Export] Area3D damage;
    [Export] Node3D highlight;

    public void Highlight()
    {
        highlight.Visible = true;
        SetProcess(false);
        damage.Monitorable = false;
    }

    public void Unfreeze()
    {
        highlight.Visible = false;
        SetProcess(true);
        damage.Monitorable = true;
    }

    public override void _EnterTree()
    {
        detection.BodyEntered += (a) => QueueFree();
    }


    public override void _Process(double delta)
    {
        if (GlobalPosition.Y <= 0.3) return;
        if (ManagersDB.GameManager.TimeFrozenFlag == false) 
            GlobalPosition += Vector3.Down * 10 * ManagersDB.GameManager.GlobalDeltaTime;
    }

    public override void _PhysicsProcess(double delta)
	{

	}
    
}

public interface IFreezable
{
    public abstract void Highlight();

    public abstract void Unfreeze();
}
