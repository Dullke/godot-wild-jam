using Godot;

public partial class TimeStopMeter : ProgressBar
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		MaxValue = ManagersDB.GameManager.maxFreezeTime;
		Value = ManagersDB.GameManager.FrozenTimeLeft;
	}
}
